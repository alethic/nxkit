using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'var' element attributes.
    /// </summary>
    [Extension(typeof(VarAttributes), "{http://www.w3.org/2002/xforms}var")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class VarAttributes :
        AttributeAccessor
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        [ImportingConstructor]
        public VarAttributes(XElement element)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
        }

        /// <summary>
        /// Gets the 'name' attribute values.
        /// </summary>
        public string Name
        {
            get { return GetAttributeValue("name"); }
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