using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'name' element attributes.
    /// </summary>
    [Extension(typeof(HeaderNameAttributes), "{http://www.w3.org/2002/xforms}name", PredicateType = typeof(HeaderNamePredicate))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class HeaderNameAttributes :
        AttributeAccessor
    {

        class HeaderNamePredicate :
            IExtensionPredicate
        {

            public bool IsMatch(XObject obj)
            {
                return obj.Parent != null && obj.Parent.Name == Constants.XForms_1_0 + "header";
            }

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        [ImportingConstructor]
        public HeaderNameAttributes(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

        /// <summary>
        /// Gets the 'value' attribute.
        /// </summary>
        public XAttribute ValueAttribute
        {
            get { return GetAttribute("value"); }
        }

        /// <summary>
        /// Gets the 'value' attribute values.
        /// </summary>
        public string Value
        {
            get { return GetAttributeValue("value"); }
        }

    }

}