using System;
using System.Xml.Linq;

using NXKit.Diagnostics;

namespace NXKit.XForms
{

    [Extension("{http://www.w3.org/2002/xforms}value", PredicateType = typeof(ItemValuePredicate))]
    public class ItemValue :
        ElementExtension,
        ISelectableValue
    {

        class ItemValuePredicate :
            IExtensionPredicate
        {

            public bool IsMatch(XObject obj)
            {
                return obj.Parent != null && obj.Parent.Name == Constants.XForms_1_0 + "item";
            }

        }

        readonly ItemValueAttributes attributes;
        readonly Lazy<IBindingNode> bindingNode;
        readonly ITraceService trace;
        readonly Lazy<Binding> valueBinding;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributes"></param>
        /// <param name="bindingNode"></param>
        /// <param name="trace"></param>
        public ItemValue(
            XElement element,
            ItemValueAttributes attributes,
            Lazy<IBindingNode> bindingNode,
            ITraceService trace)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            this.attributes = attributes ?? throw new ArgumentNullException(nameof(attributes));
            this.bindingNode = bindingNode ?? throw new ArgumentNullException(nameof(bindingNode));
            this.trace = trace ?? throw new ArgumentNullException(nameof(trace));

            valueBinding = new Lazy<Binding>(() => BindingUtil.ForAttribute(attributes.ValueAttribute, trace));
        }

        /// <summary>
        /// Gets the binding of the element.
        /// </summary>
        Binding Binding => bindingNode.Value?.Binding;

        /// <summary>
        /// Gets the 'value' attribute binding.
        /// </summary>
        Binding ValueBinding => valueBinding.Value;

        /// <summary>
        /// Gets the appropriate value to use when selecting the item.
        /// </summary>
        /// <returns></returns>
        string GetValue()
        {
            if (Binding != null)
                return Binding.Value;

            if (ValueBinding != null)
                return ValueBinding.Value;

            return Element.Value;
        }

        public void Select(UIBinding ui)
        {
            ui.Value = GetValue();
        }

        public void Deselect(UIBinding ui)
        {
            ui.Value = "";
        }

        public bool IsSelected(UIBinding ui)
        {
            var value = GetValue();
            return ui.Value == value;
        }

        public int GetValueHashCode()
        {
            return (GetValue() ?? "").GetHashCode();
        }

    }

}
