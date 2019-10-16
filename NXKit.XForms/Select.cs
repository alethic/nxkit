using System;
using System.Xml.Linq;

namespace NXKit.XForms
{

    [Extension("{http://www.w3.org/2002/xforms}select")]
    public class Select :
        ElementExtension
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Select(XElement element)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
        }

    }

}
