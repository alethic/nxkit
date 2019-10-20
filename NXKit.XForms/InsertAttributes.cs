using System;
using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'insert' element.
    /// </summary>
    [Extension(typeof(InsertAttributes), "{http://www.w3.org/2002/xforms}insert")]
    public class InsertAttributes :
        ActionAttributes
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public InsertAttributes(XElement element)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            if (element.Name != Constants.XForms + "insert")
                throw new ArgumentException(nameof(element));
        }

        /// <summary>
        /// Gets the 'origin' attribute value.
        /// </summary>
        public string Origin
        {
            get { return GetAttributeValue("origin"); }
        }

        /// <summary>
        /// Gets the 'at' attribute value.
        /// </summary>
        public string At
        {
            get { return GetAttributeValue("at"); }
        }

        /// <summary>
        /// Gets the 'position' attribute value.
        /// </summary>
        public string Postition
        {
            get { return GetAttributeValue("position"); }
        }

    }

}