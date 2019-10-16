using System;
using System.Xml.Linq;

namespace NXKit.NXInclude
{

    [Extension(typeof(IncludeProperties))]
    public class IncludeProperties :
        XInclude.IncludeProperties
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributes"></param>
        public IncludeProperties(
            XElement element,
            IncludeAttributes attributes)
            : base(element, () => attributes)
        {
            if (element is null)
                throw new ArgumentNullException(nameof(element));
            if (attributes is null)
                throw new ArgumentNullException(nameof(attributes));
        }

    }

}
