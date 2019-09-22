using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.XForms.Layout
{

    [Extension("{http://schemas.nxkit.org/2014/xforms-layout}a")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    [Remote]
    public class Anchor :
        ElementExtension
    {

        readonly AnchorAttributes attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        [ImportingConstructor]
        public Anchor(
            XElement element,
            AnchorAttributes attributes)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            this.attributes = attributes;
        }

        /// <summary>
        /// Gets the icon name.
        /// </summary>
        [Remote]
        public string Href
        {
            get { return attributes.Href; }
        }

    }

}
