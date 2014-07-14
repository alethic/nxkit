using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.Xml;

namespace NXKit.XForms
{

    [Extension("{http://www.w3.org/2002/xforms}value", PredicateType = typeof(ItemValuePredicate))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class ItemValue :
        ElementExtension,
        ISelectableValue
    {

        class ItemValuePredicate :
            IExtensionPredicate
        {

            public bool IsMatch(XObject obj, Type type)
            {
                return obj.Parent != null && obj.Parent.Name == Constants.XForms_1_0 + "item";
            }

        }

        readonly ItemValueAttributes attributes;
        readonly Lazy<IBindingNode> bindingNode;
        readonly Lazy<Binding> valueBinding;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public ItemValue(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.attributes = new ItemValueAttributes(Element);
            this.bindingNode = new Lazy<IBindingNode>(() => Element.Interface<IBindingNode>());
            this.valueBinding = new Lazy<Binding>(() => BindingUtil.ForAttribute(attributes.ValueAttribute));
        }

        /// <summary>
        /// Gets the 'value' element attributes.
        /// </summary>
        ItemValueAttributes Attributes
        {
            get { return attributes; }
        }

        /// <summary>
        /// Gets the binding of the element.
        /// </summary>
        Binding Binding
        {
            get { return bindingNode.Value != null ? bindingNode.Value.Binding : null; }
        }

        /// <summary>
        /// Gets the 'value' attribute binding.
        /// </summary>
        Binding ValueBinding
        {
            get { return valueBinding.Value; }
        }

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
