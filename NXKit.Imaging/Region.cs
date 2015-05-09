using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.Imaging
{

    [Extension(typeof(RegionProperties))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class Region :
        ElementExtension
    {

        readonly RegionProperties properties;

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
        }

    }

}
