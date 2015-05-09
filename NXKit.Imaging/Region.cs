using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.Xml;

namespace NXKit.Imaging
{

    [Extension(typeof(RegionProperties))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class Region :
        ElementExtension
    {

        readonly RegionProperties properties;
        readonly Lazy<Region> parent;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        [ImportingConstructor]
        public Region(
            XElement element,
            RegionProperties properties)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(properties != null);

            this.properties = properties;
            this.parent = new Lazy<Region>(() => element.Parent != null && element.Parent is XElement ? element.Parent.Interface<Region>() : null);
        }

        public Unit? Left
        {
            get { return properties.Left; }
        }

        public Unit? Top
        {
            get { return properties.Top; }
        }

        public Unit? Right
        {
            get { return properties.Right; }
        }

        public Unit? Bottom
        {
            get { return properties.Bottom; }
        }

        public Unit? Width
        {
            get { return properties.Width; }
        }

        public Unit? Height
        {
            get { return properties.Height; }
        }

    }

}
