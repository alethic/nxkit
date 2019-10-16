using System;
using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// When the name element appears as a child of element header, it is used to specify the name of a header entry to
    /// be provided to the submission protocol.
    /// </summary>
    [Extension("{http://www.w3.org/2002/xforms}name", PredicateType = typeof(HeaderNamePredicate))]
    public class HeaderName :
        ElementExtension
    {

        class HeaderNamePredicate :
            IExtensionPredicate
        {

            public bool IsMatch(XObject obj)
            {
                return obj.Parent != null && obj.Parent.Name == Constants.XForms_1_0 + "header";
            }

        }

        readonly HeaderNameAttributes attributes;
        readonly Lazy<IBindingNode> bindingNode;
        readonly Lazy<Binding> valueBinding;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributes"></param>
        /// <param name="bindingNode"></param>
        public HeaderName(
            XElement element,
            HeaderNameAttributes attributes,
            Lazy<IBindingNode> bindingNode)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            this.attributes = attributes;
            this.bindingNode = bindingNode;
            this.valueBinding = new Lazy<Binding>(() => BindingUtil.ForAttribute(attributes.ValueAttribute));
        }

        Binding Binding
        {
            get { return bindingNode.Value?.Binding; }
        }

        Binding ValueBinding
        {
            get { return valueBinding.Value; }
        }

        /// <summary>
        /// Gets the appropriate value to use when selecting the item.
        /// </summary>
        /// <returns></returns>
        internal string GetValue()
        {
            if (Binding != null)
                return Binding.Value;

            if (ValueBinding != null)
                return ValueBinding.Value;

            return Element.Value;
        }

    }

}
