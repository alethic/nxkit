using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.NXInclude
{

    public class IncludeAttributes :
        XInclude.IncludeAttributes
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public IncludeAttributes(XElement element)
            : base(element, "http://schemas.nxkit.org/2014/NXInclude")
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

    }

}
