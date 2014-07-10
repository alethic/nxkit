using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Diagnostics;
using NXKit.IO;

namespace NXKit.NXInclude
{

    [Interface("{http://schemas.nxkit.org/2014/NXInclude}include")]
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
        public Include(
            XElement element,
            Lazy<ITraceService> trace,
            Lazy<IIOService> io,
            Lazy<IncludeProperties> properties)
            : base(element, trace, io, new Lazy<XInclude.IncludeProperties>(() => properties.Value))
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
