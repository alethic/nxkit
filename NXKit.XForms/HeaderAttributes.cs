using System;
using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the attributes for the 'header' element.
    /// </summary>
    [Extension(typeof(HeaderAttributes), "{http://www.w3.org/2002/xforms}header")]
    public class HeaderAttributes :
        CommonAttributes
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public HeaderAttributes(XElement element)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
        }

        /// <summary>
        /// Gets the value of the 'combine' attribute.
        /// </summary>
        public string Combine
        {
            get { return GetAttributeValue("combine"); }
        }

    }

}
