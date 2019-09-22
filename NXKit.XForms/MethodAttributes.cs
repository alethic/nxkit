using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'method' element attributes.
    /// </summary>
    [Extension(typeof(MethodAttributes), "{http://www.w3.org/2002/xforms}method")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class MethodAttributes :
        AttributeAccessor
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        [ImportingConstructor]
        public MethodAttributes(XElement element)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
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