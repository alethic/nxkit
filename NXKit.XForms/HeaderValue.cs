using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.Xml;

namespace NXKit.XForms
{

    [Extension("{http://www.w3.org/2002/xforms}value", PredicateType = typeof(HeaderValuePredicate))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class HeaderValue :
        ElementExtension
    {

        class HeaderValuePredicate :
            IExtensionPredicate
        {

            public bool IsMatch(XObject obj, Type type)
            {
                return obj.Parent != null && obj.Parent.Name == Constants.XForms_1_0 + "header";
            }

        }

        readonly Extension<HeaderValueAttributes> attributes;
        readonly Extension<IBindingNode> bindingNode;
        readonly Lazy<Binding> valueBinding;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributes"></param>
        /// <param name="bindingNode"></param>
        [ImportingConstructor]
        public HeaderValue(
            XElement element,
            Extension<HeaderValueAttributes> attributes,
            Extension<IBindingNode> bindingNode)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(attributes != null);
            Contract.Requires<ArgumentNullException>(bindingNode != null);

            this.attributes = attributes;
            this.bindingNode = bindingNode;
            this.valueBinding = new Lazy<Binding>(() => BindingUtil.ForAttribute(attributes.Value.Value));
        }

        /// <summary>
        /// Gets the 'value' element attributes.
        /// </summary>
        HeaderValueAttributes Attributes
        {
            get { return attributes.Value; }
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

    }

}
