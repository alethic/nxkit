using System;
using System.Xml.Linq;

namespace NXKit.XForms
{

    [Extension("{http://www.w3.org/2002/xforms}group")]
    public class Group :
        ElementExtension
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public Group(XElement element)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
        }

    }

}
