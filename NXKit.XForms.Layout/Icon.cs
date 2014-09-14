using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.Xml;

namespace NXKit.XForms.Layout
{

    [Extension("{http://schemas.nxkit.org/2014/xforms-layout}icon")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    [Remote]
    public class Icon :
        ElementExtension
    {

        readonly Extension<IconAttributes> attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        [ImportingConstructor]
        public Icon(
            XElement element,
            Extension<IconAttributes> attributes)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.attributes = attributes;
        }

        /// <summary>
        /// Gets the icon name.
        /// </summary>
        [Remote]
        public string Name
        {
            get { return attributes.Value.Name; }
        }

    }

}
