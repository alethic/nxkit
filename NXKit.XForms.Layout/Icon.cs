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

        readonly IconAttributes attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Icon(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.attributes = element.AnnotationOrCreate(() => new IconAttributes(element));
        }

        /// <summary>
        /// Gets the icon name.
        /// </summary>
        [Remote]
        public string Name
        {
            get { return attributes.Name; }
        }

    }

}
