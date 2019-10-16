using System;
using System.Xml.Linq;

namespace NXKit.XForms
{

    [Extension("{http://www.w3.org/2002/xforms}switch")]
    public class Switch :
        ElementExtension
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Switch(XElement element)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
        }

    }

}
