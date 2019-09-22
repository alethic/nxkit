using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.XForms.Layout
{

    /// <summary>
    /// Makes the 'anchor' attribute available.
    /// </summary>
    [Extension(typeof(AnchorAttributes), "{http://schemas.nxkit.org/2014/xforms-layout}anchor")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class AnchorAttributes :
        AttributeAccessor
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        [ImportingConstructor]
        public AnchorAttributes(XElement element)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
        }

        /// <summary>
        /// Gets the 'name' attribute value.
        /// </summary>
        public string Href
        {
            get { return GetAttributeValue("href"); }
        }

    }

}
