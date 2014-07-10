using System;
using System.Diagnostics.Contracts;
using System.Text;
using System.Xml.Linq;
using NXKit.Util;
using NXKit.XInclude;
using NXKit.Xml;

namespace NXKit.NXInclude
{

    [Interface("{http://schemas.nxkit.org/2014/NXInclude}include")]
    public class IncludeProperties :
        XInclude.IncludeProperties
    {

        readonly IncludeAttributes attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public IncludeProperties(XElement element)
            : base(element, element.AnnotationOrCreate<IncludeAttributes>(() => new IncludeAttributes(element)))
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

        public bool Scope
        {
            get { return true; }
        }

    }

}
