using System;
using System.Xml.Linq;

namespace NXKit.NXInclude
{

    [Extension(typeof(IncludeAttributes))]
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
            if (element is null)
                throw new ArgumentNullException(nameof(element));
        }

    }

}
