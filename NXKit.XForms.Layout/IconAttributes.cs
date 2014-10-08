using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.XForms.Layout
{

    /// <summary>
    /// Makes the 'icon' attribute available.
    /// </summary>
    [Extension(typeof(IconAttributes), "{http://schemas.nxkit.org/2014/xforms-layout}icon")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class IconAttributes :
        AttributeAccessor
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        [ImportingConstructor]
        public IconAttributes(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

        /// <summary>
        /// Gets the 'name' attribute value.
        /// </summary>
        public string Name
        {
            get { return GetAttributeValue("name"); }
        }

    }

}
