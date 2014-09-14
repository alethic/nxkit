using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;
using NXKit.Composition;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'resource' element attributes.
    /// </summary>
    [Extension("{http://www.w3.org/2002/xforms}resource")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class ResourceAttributes :
        AttributeAccessor
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        [ImportingConstructor]
        public ResourceAttributes(XElement element)
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