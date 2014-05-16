using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

using NXKit.DOMEvents;
using NXKit.Xml;
using NXKit.XMLEvents;

namespace NXKit.XForms
{

    [Interface("{http://www.w3.org/2002/xforms}setvalue")]
    public class SetValue :
        ElementExtension,
        IEventHandler
    {
        readonly Lazy<CommonProperties> commonProperties;
        readonly Lazy<BindingProperties> bindingProperties;
        readonly Lazy<SetValueProperties> insertProperties;
        readonly Lazy<EvaluationContextResolver> contextResolver;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public SetValue(
            XElement element,
            Lazy<CommonProperties> commonProperties,
            Lazy<BindingProperties> bindingProperties,
            Lazy<SetValueProperties> setValueProperties,
            Lazy<EvaluationContextResolver> contextResolver)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(commonProperties != null);
            Contract.Requires<ArgumentNullException>(bindingProperties != null);
            Contract.Requires<ArgumentNullException>(setValueProperties != null);
            Contract.Requires<ArgumentNullException>(contextResolver != null);

            this.commonProperties = commonProperties;
            this.bindingProperties = bindingProperties;
            this.insertProperties = setValueProperties;
            this.contextResolver = contextResolver;
        }

        EvaluationContext GetContext()
        {
            var context = contextResolver.Value.GetInScopeEvaluationContext();
            if (commonProperties.Value.Context != null)
            {
                var item = new Binding(Element, context, commonProperties.Value.Context).ModelItems.FirstOrDefault();
                if (item == null)
                    return null;

                context = new EvaluationContext(item.Model, item.Instance, item, 1, 1);
            }

            return context;
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
