using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.DOMEvents;
using NXKit.Xml;
using NXKit.XMLEvents;

namespace NXKit.XForms
{

    [Extension("{http://www.w3.org/2002/xforms}setvalue")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class SetValue :
        ElementExtension,
        IEventHandler
    {

        readonly CommonProperties commonProperties;
        readonly BindingProperties bindingProperties;
        readonly SetValueProperties setValueProperties;
        readonly Extension<EvaluationContextResolver> context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="commonProperties"></param>
        /// <param name="bindingProperties"></param>
        /// <param name="setValueProperties"></param>
        /// <param name="context"></param>
        [ImportingConstructor]
        public SetValue(
            XElement element,
            CommonProperties commonProperties,
            BindingProperties bindingProperties,
            SetValueProperties setValueProperties,
            Extension<EvaluationContextResolver> context)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(commonProperties != null);
            Contract.Requires<ArgumentNullException>(bindingProperties != null);
            Contract.Requires<ArgumentNullException>(setValueProperties != null);
            Contract.Requires<ArgumentNullException>(context != null);

            this.commonProperties = commonProperties;
            this.bindingProperties = bindingProperties;
            this.setValueProperties = setValueProperties;
            this.context = context;
        }

        EvaluationContext GetContext()
        {
            if (commonProperties.Context != null)
            {
                var item = new Binding(Element, context.Value.GetInScopeEvaluationContext(), commonProperties.Context).ModelItems.FirstOrDefault();
                if (item == null)
                    return null;

                return new EvaluationContext(item.Model, item.Instance, item, 1, 1);
            }

            return null;
        }

        XObject[] GetSequenceBindingNodeSequence(EvaluationContext insertContext)
        {
            Contract.Requires<ArgumentNullException>(insertContext != null);
            Contract.Ensures(Contract.Result<XObject[]>() != null);

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

            var ref_ = bindingProperties.Ref ?? bindingProperties.NodeSet;
            if (ref_ != null)
                return new Binding(Element, insertContext, ref_).ModelItems
                    .Select(i => i.Xml)
                    .ToArray();

            return new XObject[0];
        }

        public void HandleEvent(Event ev)
        {
            var context = GetContext();
            if (context == null)
                return;

            var sequenceBindingNodeSequence = GetSequenceBindingNodeSequence(context);
            if (sequenceBindingNodeSequence.Length == 0)
                return;

            var insertNode = sequenceBindingNodeSequence[0];
            var insertItem = ModelItem.Get(insertNode);
            if (insertItem.ReadOnly)
                return;

            if (insertNode is XElement)
                if (((XElement)insertNode).HasElements)
                    throw new DOMTargetEventException(Element, Events.BindingException);

            insertItem.Value = Element.Value;
            insertItem.Model.State.Recalculate = true;
            insertItem.Model.State.Revalidate = true;
            insertItem.Model.State.Refresh = true;
        }

    }

}
