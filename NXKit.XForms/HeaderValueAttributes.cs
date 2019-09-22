using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'value' element attributes.
    /// </summary>
    [Extension(typeof(HeaderValueAttributes), "{http://www.w3.org/2002/xforms}value", PredicateType = typeof(HeaderValuePredicate))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class HeaderValueAttributes :
        AttributeAccessor
    {

        class HeaderValuePredicate :
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
        public HeaderValueAttributes(XElement element)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
        }

        /// <summary>
        /// Gets the 'value' attribute value.
        /// </summary>
        public XAttribute Value
        {
            get { return GetAttribute("value"); }
        }

    }

}