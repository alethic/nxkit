using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

using NXKit.DOMEvents;
using NXKit.Xml;
using NXKit.XMLEvents;

namespace NXKit.XForms
{

    [Interface("{http://www.w3.org/2002/xforms}insert")]
    public class Insert :
        ElementExtension,
        IEventHandler
    {

        enum TargetPosition
        {

            Undefined,
            Before,
            After,
            Node,

        }

        struct TargetLocation
        {

            readonly XObject source;
            readonly XObject target;
            readonly TargetPosition position;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="source"></param>
            /// <param name="position"></param>
            public TargetLocation(XObject source, TargetPosition position)
                : this()
            {
                Contract.Requires<ArgumentNullException>(source != null);

                this.source = source;
                this.position = position;
            }

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="source"></param>
            /// <param name="position"></param>
            /// <param name="target"></param>
            public TargetLocation(XObject source, TargetPosition position, XObject target)
                : this(source, position)
            {
                Contract.Requires<ArgumentNullException>(source != null);
                Contract.Requires<ArgumentNullException>(target != null);

                this.target = target;
            }

            public XObject Source
            {
                get { return source; }
            }

            public TargetPosition Position
            {
                get { return position; }
            }

            public XObject Target
            {
                get { return target; }
            }

        }


        readonly Lazy<CommonProperties> commonProperties;
        readonly Lazy<BindingProperties> bindingProperties;
        readonly Lazy<InsertProperties> insertProperties;
        readonly Lazy<EvaluationContextResolver> contextResolver;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="commonProperties"></param>
        /// <param name="bindingProperties"></param>
        /// <param name="insertProperties"></param>
        /// <param name="contextResolver"></param>
        public Insert(
            XElement element,
            Lazy<CommonProperties> commonProperties,
            Lazy<BindingProperties> bindingProperties,
            Lazy<InsertProperties> insertProperties,
            Lazy<EvaluationContextResolver> contextResolver)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(commonProperties != null);
            Contract.Requires<ArgumentNullException>(bindingProperties != null);
            Contract.Requires<ArgumentNullException>(insertProperties != null);
            Contract.Requires<ArgumentNullException>(contextResolver != null);

            this.commonProperties = commonProperties;
            this.bindingProperties = bindingProperties;
            this.insertProperties = insertProperties;
            this.contextResolver = contextResolver;
        }

        public void HandleEvent(Event ev)
        {
            Invoke();
        }

        /// <summary>
        /// The insert context is determined. If the context attribute is not given, the insert context is the 
        /// in-scope evaluation context. Otherwise, the expression provided by the context attribute is evaluated
        /// using the in-scope evaluation context, and the first-item rule is applied to obtain the insert context.
        /// The insert action is terminated with no effect if the insert context is the empty sequence.
        /// </summary>
        /// <returns></returns>
        EvaluationContext GetInsertContext()
        {
            var insertContext = contextResolver.Value.GetInScopeEvaluationContext();
            if (commonProperties.Value.Context != null)
            {
                var item = new Binding(Element, insertContext, commonProperties.Value.Context).ModelItems.FirstOrDefault();
                if (item == null)
                    return null;

                insertContext = new EvaluationContext(item.Model, item.Instance, item, 1, 1);
            }

            return insertContext;
        }

        /// <summary>
        /// Gets the 'Sequence Binding node-sequence'.
        /// </summary>
        /// <param name="insertContext"></param>
        /// <returns></returns>
        XObject[] GetSequenceBindingNodeSequence(EvaluationContext insertContext)
        {
            Contract.Requires<ArgumentNullException>(insertContext != null);
            Contract.Ensures(Contract.Result<XObject[]>() != null);

            // If a bind attribute is present, it directly determines the Sequence Binding node-sequence.
            var bindId = bindingProperties.Value.Bind;
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
            var ref_ = bindingProperties.Value.Ref ?? bindingProperties.Value.NodeSet;
            if (ref_ != null)
                return new Binding(Element, insertContext, ref_).ModelItems
                    .Select(i => i.Xml)
                    .ToArray();

            // If the Sequence Binding attributes are not present, then the Sequence Binding node-sequence is the
            // empty sequence.
            return new XObject[0];
        }

        /// <summary>
        /// Gets the 'origin node-sequence'.
        /// </summary>
        /// <param name="insertContext"></param>
        /// <param name="sequenceBindingNodeSequence"></param>
        /// <returns></returns>
        XObject[] GetOriginNodeSequence(EvaluationContext insertContext, XObject[] sequenceBindingNodeSequence)
        {
            Contract.Requires<ArgumentNullException>(insertContext != null);
            Contract.Requires<ArgumentNullException>(sequenceBindingNodeSequence != null);
            Contract.Ensures(Contract.Result<XObject[]>() != null);

            XObject[] result = null;

            // If the origin attribute is not given and the Sequence Binding sequence is empty, then the origin
            // node-sequence is the empty sequence.
            if (insertProperties.Value.Origin == null &&
                sequenceBindingNodeSequence.Length == 0)
                result = new XObject[0];

            // Otherwise, if the origin attribute is not given, then the origin node-sequence consists of the last
            // node of the Sequence Binding node-sequence.
            else if (insertProperties.Value.Origin == null)
                result = new[] { sequenceBindingNodeSequence[sequenceBindingNodeSequence.Length - 1] };

            // If the origin attribute is given, the origin node-sequence is the result of the evaluation of the origin
            // attribute in the insert context.
            else if (insertProperties.Value.Origin != null)
                result = new Binding(Element, insertContext, insertProperties.Value.Origin).ModelItems
                    .Select(i => i.Xml)
                    .ToArray();

            else
                result = new XObject[0];

            // preempt empty result set
            if (result.Length == 0)
                return result;

            // Namespace nodes and root nodes (parents of document elements) are removed from the origin node-sequence.
            return result
                .Where(i => i.NodeType != XmlNodeType.Attribute || !((XAttribute)i).IsNamespaceDeclaration)
                .Where(i => i.Parent != null || i == i.Document.Root)
                .ToArray();
        }

        /// <summary>
        /// Gets the 'insert location node'.
        /// </summary>
        /// <param name="insertContext"></param>
        /// <param name="sequenceBindingNodeSequence"></param>
        /// <returns></returns>
        XObject GetInsertLocationNode(EvaluationContext insertContext, XObject[] sequenceBindingNodeSequence)
        {
            Contract.Requires<ArgumentNullException>(insertContext != null);
            Contract.Requires<ArgumentNullException>(sequenceBindingNodeSequence != null);
            Contract.Ensures(Contract.Result<XObject>() != null);

            // If the Sequence Binding node-sequence is not specified or empty, the insert location node is the insert
            // context node.
            if (sequenceBindingNodeSequence.Length == 0)
                return insertContext.ModelItem.Xml;

            // Otherwise, if the at attribute is not given, then the insert location node is the last node of the
            // Sequence Binding sequence.
            else if (insertProperties.Value.At == null)
                return sequenceBindingNodeSequence[sequenceBindingNodeSequence.Length - 1];

            // Otherwise, an insert location node is determined from the at attribute as follows:
            else
            {
                // 1. The evaluation context node is the first node in document order from the Sequence Binding
                // node-sequence, the context size is the size of the Sequence Binding node-sequence, and the context
                // position is 1.
                var at = new Binding(
                    Element,
                    new EvaluationContext(ModelItem.Get(sequenceBindingNodeSequence[0]), 1, sequenceBindingNodeSequence.Length),
                    insertProperties.Value.At).Value;

                // 2. The return value is processed according to the rules of the XPath function round(). For example,
                // the literal 1.5 becomes 2, and the literal 'string' becomes NaN.
                double location;
                if (double.TryParse(at, out location))
                    location = Math.Round(location, 0, MidpointRounding.AwayFromZero);
                else
                    location = double.NaN;

                // 3. If the result is in the range 1 to the Sequence Binding node-sequence size, then the insert
                // location is equal to the result. If the result is non-positive, then the insert location is 1.
                // Otherwise, the result is NaN or exceeds the Sequence Binding sequence size, so the insert location
                // is the Sequence Binding sequence size.
                if (location <= 0)
                    location = 1;
                else if (location == double.NaN || location > sequenceBindingNodeSequence.Length)
                    location = sequenceBindingNodeSequence.Length;

                // 4. The insert location node is the node in the Sequence Binding sequence at the position given by
                // the insert location.
                return sequenceBindingNodeSequence[(int)location - 1];
            }
        }

        /// <summary>
        /// Gets the 'target location'.
        /// </summary>
        /// <param name="insertContext"></param>
        /// <param name="sequenceBindingNodeSequence"></param>
        /// <param name="insertLocationNode"></param>
        /// <param name="insertNode"></param>
        /// <returns></returns>
        TargetLocation GetTargetLocation(EvaluationContext insertContext, XObject[] sequenceBindingNodeSequence, XObject insertLocationNode, XObject insertNode)
        {
            Contract.Requires<ArgumentNullException>(insertContext != null);
            Contract.Requires<ArgumentNullException>(sequenceBindingNodeSequence != null);
            Contract.Requires<ArgumentNullException>(insertLocationNode != null);
            Contract.Requires<ArgumentNullException>(insertNode != null);

            // f the Sequence Binding node-sequence is not specified or empty, then the insert location node provided
            // by the context attribute is intended to be the parent of the cloned node. The target location is
            // dependent on the types of the cloned node and the insert location node as follows:
            if (sequenceBindingNodeSequence.Length == 0)
            {

                // If the insert location node is not an element node or root node, then it cannot be the parent of the
                // cloned node, so the target location is undefined.
                if (insertLocationNode.NodeType != XmlNodeType.Element && insertLocationNode.Parent != null)
                    return new TargetLocation(insertNode, TargetPosition.Undefined);

                // If the insert location node is the root node of an instance (which is the parent of the root element),
                // and the cloned node is an element, then the target location is the root element of the instance.
                if (insertLocationNode == insertLocationNode.Document.Root &&
                    insertNode.NodeType == XmlNodeType.Element)
                {
                    var target = insertLocationNode.Document.Root;
                    if (target == null)
                        throw new InvalidOperationException();

                    if (target.IsEmpty)
                        return new TargetLocation(insertNode, TargetPosition.Node, target);
                    else
                        return new TargetLocation(insertNode, TargetPosition.After, target.LastNode);
                }

                // If the insert location node is the root node of an instance (which is the parent of the root element),
                // and the cloned node is not an element, then the target location is before the first child of the insert
                // location node.
                if (insertLocationNode == insertLocationNode.Document.Root &&
                    insertNode.NodeType != XmlNodeType.Element)
                    if (((XElement)insertLocationNode).IsEmpty)
                        return new TargetLocation(insertNode, TargetPosition.Node, insertLocationNode);
                    else
                        return new TargetLocation(insertNode, TargetPosition.Before, ((XElement)insertLocationNode).FirstNode);

                // If the insert location node is an element, and the cloned node is an attribute, then the target
                // location is the attribute list of the insert location node.
                if (insertLocationNode.NodeType == XmlNodeType.Element &&
                    insertNode.NodeType == XmlNodeType.Attribute)
                    return new TargetLocation(insertNode, TargetPosition.Node, insertLocationNode);

                // If the insert location node is an element, and the cloned node is not an attribute, then the target
                // location is before the first child of the insert location node, or the child list of the insert
                // location node if it is empty.
                if (insertLocationNode.NodeType == XmlNodeType.Element &&
                    insertNode.NodeType != XmlNodeType.Attribute)
                    if (((XElement)insertLocationNode).IsEmpty)
                        return new TargetLocation(insertNode, TargetPosition.Node, insertLocationNode);
                    else
                        return new TargetLocation(insertNode, TargetPosition.Before, ((XElement)insertLocationNode).FirstNode);
            }
            else
            {
                // Otherwise, the Sequence Binding node-sequence is specified and non-empty, so the insert location
                // node provided by the Sequence Binding and author-optional at attribute is intended to be the sibling
                // of the cloned node.  

                // If the insert location node is an attribute or root node, then the target location is undefined.
                if (insertLocationNode == insertLocationNode.Document.Root ||
                    insertLocationNode.NodeType == XmlNodeType.Attribute)
                    return new TargetLocation(insertNode, TargetPosition.Undefined);

                // If the insert location node is not an attribute or root node, then the
                // target location is immediately before or after the insert location node, based on the position
                // attribute setting or its default.
                else if (insertProperties.Value.Position == InsertPosition.After)
                    return new TargetLocation(insertNode, TargetPosition.After, insertLocationNode);
                else if (insertProperties.Value.Position == InsertPosition.Before)
                    return new TargetLocation(insertNode, TargetPosition.Before, insertLocationNode);
            }

            // should never reach this point
            throw new InvalidOperationException();
        }

        XObject InsertTarget(TargetLocation target)
        {
            if (target.Position == TargetPosition.Undefined)
                return null;

            var sourceAttr = target.Source as XAttribute;
            if (sourceAttr != null)
            {
                var targetNode = target.Target as XElement;
                if (targetNode != null)
                {
                    var attr = targetNode.Attribute(sourceAttr.Name);
                    if (attr != null)
                    {
                        attr.Value = sourceAttr.Value;
                        return attr;
                    }

                    targetNode.Add(sourceAttr);
                    return sourceAttr;
                }

                var targetAttr = target.Target as XAttribute;
                if (targetAttr != null)
                {
                    if (target.Position == TargetPosition.Node)
                    {
                        targetAttr.Value = sourceAttr.Value;
                        return targetAttr;
                    }

                    if (target.Position == TargetPosition.Before)
                    {
                        throw new InvalidOperationException();
                    }

                    if (target.Position == TargetPosition.After)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }

            var sourceNode = target.Source as XNode;
            if (sourceNode != null)
            {
                var targetCntr = target.Target as XContainer;
                if (targetCntr != null)
                {
                    if (target.Position == TargetPosition.Node)
                    {
                        targetCntr.Add(sourceNode);
                        return sourceNode;
                    }
                }

                var targetNode = target.Target as XNode;
                if (targetNode != null)
                {

                    if (target.Position == TargetPosition.Before)
                    {
                        targetNode.AddBeforeSelf(sourceNode);
                        return sourceNode;
                    }

                    if (target.Position == TargetPosition.After)
                    {
                        targetNode.AddAfterSelf(sourceNode);
                        return sourceNode;
                    }
                }
            }

            throw new InvalidOperationException();
        }

        public void Invoke()
        {
            var insertContext = GetInsertContext();
            if (insertContext == null)
                return;

            var sequenceBindingNodeSequence = GetSequenceBindingNodeSequence(insertContext);

            // 1. The context attribute is not given and the Sequence Binding node-sequence is the empty sequence.
            if (commonProperties.Value.Context == null &&
                sequenceBindingNodeSequence.Length == 0)
                return;

            // 2. The context attribute is given, the insert context does not evaluate to an element node and the
            //    Sequence Binding node-sequence is the empty sequence.
            if (commonProperties.Value.Context != null &&
                sequenceBindingNodeSequence.Length == 0 &&
                insertContext.ModelItem.Xml.NodeType != XmlNodeType.Element)
                return;

            // The insert action is terminated with no effect if the origin node-sequence is the empty sequence.
            var originNodeSequence = GetOriginNodeSequence(insertContext, sequenceBindingNodeSequence);
            if (originNodeSequence.Length == 0)
                return;

            var insertLocationNode = GetInsertLocationNode(insertContext, sequenceBindingNodeSequence);

            // The insert action is terminated with no effect if the insertion will create nodes whose parent is
            // readonly. This occurs if the insert location node is readonly and the Sequence Binding sequence is not
            // specified or empty, or otherwise if the parent of the insert location node is readonly.
            if (ModelItem.Get(insertLocationNode).ReadOnly)
                return;

            // The target location of each of the cloned nodes is determined.
            var targets = originNodeSequence
                .Select(i => GetTargetLocation(insertContext, sequenceBindingNodeSequence, insertLocationNode, i.Clone()))
                .ToArray();

            // The cloned node or nodes are inserted in the order they were cloned into their target locations
            // depending on their node type.
            var inserts = targets
                .Select(i => InsertTarget(i))
                .ToArray();

            // 9. If the list of inserted-nodes is empty, then the insert action is terminated with no effect.
            if (inserts.Length == 0)
                return;

            // 10. The XForms action system's deferred update flags for rebuild, recalculate, revalidate and refresh are set.
            insertContext.Model.State.Rebuild = true;
            insertContext.Model.State.Recalculate = true;
            insertContext.Model.State.Revalidate = true;
            insertContext.Model.State.Refresh = true;

            // 11. The insert action is successfully completed by dispatching the xforms-insert event with appropriate
            // context information.
            insertContext.Instance.Element.Interface<INXEventTarget>()
                .DispatchEvent(Events.Insert, inserts);
        }

    }

}
