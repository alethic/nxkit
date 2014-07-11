using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;
using NXKit;
using NXKit.Composition;
using NXKit.Diagnostics;
using NXKit.IO;

namespace NXKit.NXInclude
{

    [ElementExtension("{http://schemas.nxkit.org/2014/NXInclude}include")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class Include :
        NXKit.XInclude.Include
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="trace"></param>
        /// <param name="io"></param>
        /// <param name="properties"></param>
        [ImportingConstructor]
        public Include(
            XElement element,
            Lazy<ITraceService> trace,
            Lazy<IIOService> io,
            IExtension<XElement, IncludeProperties> properties)
            : base(element, trace, io, () => properties.Value)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(trace != null);
            Contract.Requires<ArgumentNullException>(io != null);
            Contract.Requires<ArgumentNullException>(properties != null);
        }

        public new IncludeProperties Properties
        {
            get { return (IncludeProperties)base.Properties; }
        }

        protected override void PostInsertXml(XElement xml)
        {
            xml.AddAnnotation(new IncludeScopeAnnotation());
        }

    }

}
