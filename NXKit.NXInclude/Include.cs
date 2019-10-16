using System;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.Diagnostics;
using NXKit.IO;

namespace NXKit.NXInclude
{

    [Extension("{http://schemas.nxkit.org/2014/NXInclude}include")]
    [Extension(typeof(IOnInit), "{http://schemas.nxkit.org/2014/NXInclude}include")]
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
            IExport<IncludeProperties> properties,
            ITraceService trace,
            IIOService io)
            : base(element, () => properties.Value, trace, io)
        {
            if (element is null)
                throw new ArgumentNullException(nameof(element));
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));
            if (trace is null)
                throw new ArgumentNullException(nameof(trace));
            if (io is null)
                throw new ArgumentNullException(nameof(io));
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
