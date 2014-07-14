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

        readonly Lazy<EvaluationContextResolver> context;
        readonly Lazy<CommonProperties> commonProperties;
        readonly Lazy<BindingProperties> bindingProperties;
        readonly Lazy<SetValueProperties> setValueProperties;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public SetValue(
            XElement element,
            Lazy<EvaluationContextResolver> context)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(context != null);

            this.context = context;
            this.commonProperties = new Lazy<CommonProperties>(() => new CommonProperties(element, context));
            this.bindingProperties = new Lazy<BindingProperties>(() => new BindingProperties(element, context));
            this.setValueProperties = new Lazy<SetValueProperties>(() => new SetValueProperties(element, context));
        }

        EvaluationContext GetContext()
        {
            if (commonProperties.Value.Context != null)
            {
                var item = new Binding(Element, context.Value.GetInScopeEvaluationContext(), commonProperties.Value.Context).ModelItems.FirstOrDefault();
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

            var ref_ = bindingProperties.Value.Ref ?? bindingProperties.Value.NodeSet;
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
