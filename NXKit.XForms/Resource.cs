using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Xml;

namespace NXKit.XForms
{

    /// <summary>
    /// The resource element (deprecated in favor of using an AVT in the resource attribute) allows the URI used for
    /// a submission to be dynamically calculated based on instance data.
    /// </summary>
    [Interface("{http://www.w3.org/2002/xforms}resource")]
    public class Resource :
        ElementExtension
    {

        readonly ResourceAttributes attributes;
        readonly Lazy<IBindingNode> bindingNode;
        readonly Lazy<Binding> valueBinding;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Resource(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(element.Name == Constants.XForms_1_0 + "resource");

            this.attributes = new ResourceAttributes(element);
            this.bindingNode = new Lazy<IBindingNode>(() => Element.Interface<IBindingNode>());
            this.valueBinding = new Lazy<Binding>(() => BindingUtil.ForAttribute(attributes.ValueAttribute));
        }

        ResourceAttributes Attributes
        {
            get { return attributes; }
        }

        Binding Binding
        {
            get { return bindingNode.Value != null ? bindingNode.Value.Binding : null; }
        }

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

        public Uri Uri
        {
            get { return GetUri(); }
        }

        Uri GetUri()
        {
            var value = GetValue();
            if (!string.IsNullOrWhiteSpace(value))
                return new Uri(Element.GetBaseUri(), new Uri(value, UriKind.RelativeOrAbsolute));

            return null;
        }

    }

}
