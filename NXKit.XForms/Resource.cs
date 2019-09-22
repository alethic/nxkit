using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.Xml;

namespace NXKit.XForms
{

    /// <summary>
    /// The resource element (deprecated in favor of using an AVT in the resource attribute) allows the URI used for
    /// a submission to be dynamically calculated based on instance data.
    /// </summary>
    [Extension("{http://www.w3.org/2002/xforms}resource")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class Resource :
        ElementExtension
    {

        readonly ResourceAttributes attributes;
        readonly Extension<IBindingNode> bindingNode;
        readonly Lazy<Binding> valueBinding;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributes"></param>
        /// <param name="bindingNode"></param>
        [ImportingConstructor]
        public Resource(
            XElement element,
            ResourceAttributes attributes,
            Extension<IBindingNode> bindingNode)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            this.attributes = attributes ?? throw new ArgumentNullException(nameof(attributes));
            this.bindingNode = bindingNode ?? throw new ArgumentNullException(nameof(bindingNode));
            this.valueBinding = new Lazy<Binding>(() => BindingUtil.ForAttribute(attributes.ValueAttribute));
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
