using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.XForms
{

    [Interface("{http://www.w3.org/2002/xforms}value")]
    public class ItemValue :
        ISelectableValue
    {

        readonly XElement element;
        readonly ItemValueAttributes attributes;
        readonly Lazy<IBindingNode> nodeBinding;
        readonly Lazy<Binding> valueBinding;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public ItemValue(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
            this.attributes = new ItemValueAttributes(element);
            this.nodeBinding = new Lazy<IBindingNode>(() => element.Interface<IBindingNode>());
            this.valueBinding = new Lazy<Binding>(() => BindingUtil.ForAttribute(attributes.ValueAttribute));
        }

        /// <summary>
        /// Gets the 'value' element.
        /// </summary>
        public XElement Element
        {
            get { return element; }
        }

        /// <summary>
        /// Gets the 'value' element attributes.
        /// </summary>
        public ItemValueAttributes Attributes
        {
            get { return attributes; }
        }

        /// <summary>
        /// Gets the binding of the element.
        /// </summary>
        public Binding Binding
        {
            get { return nodeBinding.Value != null ? nodeBinding.Value.Binding : null; }
        }

        /// <summary>
        /// Gets the 'value' attribute binding.
        /// </summary>
        public Binding ValueBinding
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

            return element.Value;
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
            return ui.Value == GetValue();
        }

        public int GetValueHashCode()
        {
            return (GetValue() ?? "").GetHashCode();
        }

    }

}
