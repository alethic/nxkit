using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.Xml;

namespace NXKit.XForms
{

    /// <summary>
    /// When the name element appears as a child of element header, it is used to specify the name of a header entry to
    /// be provided to the submission protocol.
    /// </summary>
    [Extension("{http://www.w3.org/2002/xforms}name", PredicateType = typeof(HeaderNamePredicate))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class HeaderName :
        ElementExtension
    {

        class HeaderNamePredicate :
            IExtensionPredicate
        {

            public bool IsMatch(XObject obj, Type type)
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
        public HeaderName(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(element.Name == Constants.XForms_1_0 + "name");

            this.attributes = new HeaderNameAttributes(element);
            this.bindingNode = new Lazy<IBindingNode>(() => Element.Interface<IBindingNode>());
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
