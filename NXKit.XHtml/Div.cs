using System;
using System.Xml.Linq;

namespace NXKit.XHtml
{

    [Extension("{http://www.w3.org/1999/xhtml}div")]
    public class Div : ElementExtension
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Div(XElement element) : base(element)
        {
            if (element is null)
                throw new ArgumentNullException(nameof(element));
        }

    }

}
