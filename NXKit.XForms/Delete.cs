using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.DOMEvents;
using NXKit.Xml;
using NXKit.XMLEvents;

namespace NXKit.XForms
{

    [Extension("{http://www.w3.org/2002/xforms}delete")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class Delete :
        ElementExtension,
        IEventHandler
    {

        readonly CommonProperties commonProperties;
        readonly BindingProperties bindingProperties;
        readonly DeleteProperties deleteProperties;
        readonly Lazy<EvaluationContextResolver> contextResolver;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Delete(
            XElement element,
            Lazy<EvaluationContextResolver> contextResolver)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(contextResolver != null);

            this.contextResolver = contextResolver;
            this.commonProperties = element.AnnotationOrCreate(() => new CommonProperties(element, contextResolver));
            this.bindingProperties = element.AnnotationOrCreate(() => new BindingProperties(element, contextResolver));
            this.deleteProperties = element.AnnotationOrCreate(() => new DeleteProperties(element, contextResolver));
        }

        public void HandleEvent(Event ev)
        {
            Invoke();
        }

        /// <summary>
        /// The delete context is determined. It is set to the in-scope evaluation context, possibly overridden by the
        /// context attribute if that attribute is present. The delete action is terminated with no effect if the
        /// delete context is the empty sequence.
        /// </summary>
        /// <returns></returns>
        EvaluationContext GetDeleteContext()
        {
            var deleteContext = contextResolver.Value.GetInScopeEvaluationContext();
            if (commonProperties.Context != null)
            {
                var item = new Binding(Element, deleteContext, commonProperties.Context).ModelItems.First();
                if (item == null)
                    return null;

                deleteContext = new EvaluationContext(item.Model, item.Instance, item, 1, 1);
            }

            return deleteContext;
        }

        /// <summary>
        /// Gets the 'Sequence Binding node-sequence'.
        /// </summary>
        /// <param name="deleteContext"></param>
        /// <returns></returns>
        XObject[] GetSequenceBindingNodeSequence(EvaluationContext deleteContext)
        {
            Contract.Requires<ArgumentNullException>(deleteContext != null);
            Contract.Ensures(Contract.Result<XObject[]>() != null);

            // If a bind attribute is present, it directly determines the Sequence Binding node-sequence.
            var bindId = bindingProperties.Bind;
            if (bindId != null)
            {
                var element = Element.ResolveId(bindId);
                if (element != null)
                {
                    var bind = element.InterfaceOrDefault<Bind>();
                    if (bind == null)
                        throw new DOMTargetEventException(Element, Events.BindingException);

                    return bind.GetBoundNodes()
                        .Select(i => i.Xml)
                        .ToArray();
                }
            }

            // If a ref (or deprecated nodeset) attribute is present, it is evaluated within the insert context to
            // determine the Sequence Binding node-sequence.
            var ref_ = bindingProperties.Ref ?? bindingProperties.NodeSet;
            if (ref_ != null)
                return new Binding(Element, deleteContext, ref_).ModelItems
                    .Select(i => i.Xml)
                    .ToArray();

            // Otherwise, the Sequence Binding is not expressed, so the Sequence Binding node-sequence is set equal to 
            // the delete context node with a position and size of 1.
            return deleteContext.ModelItem != null ? new XObject[] { deleteContext.ModelItem.Xml } : new XObject[0];
        }

        public void Invoke()
        {
            var deleteContext = GetDeleteContext();
            if (deleteContext == null)
                return;

            // The Sequence Binding node-sequence is determined.
            var sequenceBindingNodeSequence = GetSequenceBindingNodeSequence(deleteContext);

            // The delete action is terminated with no effect if the Sequence Binding is expressed and the Sequence
            // Binding node-sequence is the empty sequence.
            if (sequenceBindingNodeSequence != null &&
                sequenceBindingNodeSequence.Length == 0)
                return;

            // The behavior of the delete action is undefined if the Sequence Binding node-sequence contains nodes
            // from more than one instance.
            if (sequenceBindingNodeSequence != null &&
                sequenceBindingNodeSequence.Select(i => ModelItem.Get(i).Instance).Distinct().Count() > 1)
                return;

            // Otherwise, the Sequence Binding is not expressed, so the Sequence Binding node-sequence is set equal to
            // the delete context node with a position and size of 1.
            if (sequenceBindingNodeSequence == null)
                sequenceBindingNodeSequence = new XObject[] { deleteContext.ModelItem.Xml };

            // The delete location is determined. If the at attribute is not specified, there is no delete location.
            // Otherwise, the delete location is determined by evaluating the expression specified by the at attribute
            // as follows:
            var deleteLocation = 0d;
            if (deleteProperties.At != null)
            {
                // 1. The evaluation context node is the first node in document order from the Sequence Binding
                // node-sequence, the context size is the size of the Sequence Binding node-sequence, and the context
                // position is 1.
                var at = new Binding(
                    Element,
                    new EvaluationContext(ModelItem.Get(sequenceBindingNodeSequence[0]), 1, sequenceBindingNodeSequence.Length),
                    deleteProperties.At).Value;

                // 2. The return value is processed according to the rules of the XPath function round(). For example,
                // the literal 1.5 becomes 2, and the literal 'string' becomes NaN.
                if (double.TryParse(at, out deleteLocation))
                    deleteLocation = Math.Round(deleteLocation, 0, MidpointRounding.AwayFromZero);
                else
                    deleteLocation = double.NaN;

                // 3. If the result is in the range 1 to the Sequence Binding node-sequence size, then the delete
                // location is equal to the result. If the result is non-positive, then the delete location is 1.
                // Otherwise, if the result is NaN or exceeds the Sequence Binding node-sequence size, the delete
                // location is the Sequence Binding node-sequence size.
                if (deleteLocation <= 0)
                    deleteLocation = 1;
                else if (deleteLocation == double.NaN || deleteLocation > sequenceBindingNodeSequence.Length)
                    deleteLocation = sequenceBindingNodeSequence.Length;
            }


            // If there is no delete location, each node in the Sequence Binding node-sequence is deleted, except if
            // the node is a readonly node, a namespace node, a root node, or the root document element of an instance,
            // then that particular node is not deleted. Otherwise, if there is a delete location, the node at the
            // delete location in the Sequence Binding node-sequence is deleted, except if the node is the root
            // document element of an instance or has a readonly parent node, then that node is not deleted.
            var deleteCount = 0;
            var deleteNodes = deleteLocation == 0d ? sequenceBindingNodeSequence : new XObject[] { sequenceBindingNodeSequence[(int)deleteLocation - 1] };
            foreach (var deleteNode in deleteNodes)
            {
                if (ModelItem.Get(deleteNode).ReadOnly)
                    continue;

                if (deleteNode.NodeType == XmlNodeType.Attribute)
                    if (((XAttribute)deleteNode).IsNamespaceDeclaration)
                        continue;

                if (deleteNode.Parent == null)
                    continue;

                if (deleteNode is XAttribute)
                    ((XAttribute)deleteNode).Remove();
                else if (deleteNode is XNode)
                    ((XNode)deleteNode).Remove();
                else
                    throw new InvalidOperationException();

                deleteCount++;
            }

            // The delete action is terminated with no effect if no node is deleted.
            if (deleteCount == 0)
                return;

            // 5. The XForms action system's deferred update flags for rebuild, recalculate, revalidate and refresh are
            // set.
            deleteContext.Model.State.Rebuild = true;
            deleteContext.Model.State.Recalculate = true;
            deleteContext.Model.State.Revalidate = true;
            deleteContext.Model.State.Refresh = true;

            // 6. The delete action is successfully completed by dispatching the xforms-delete event with appropriate
            // context information.
            deleteContext.Instance.Element.Interface<EventTarget>()
                .Dispatch(Events.Delete);
        }

    }

}
