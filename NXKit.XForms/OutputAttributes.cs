using System;
using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'output' element attributes.
    /// </summary>
    [Extension(typeof(OutputAttributes), "{http://www.w3.org/2002/xforms}output")]
    public class OutputAttributes :
        AttributeAccessor
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public OutputAttributes(XElement element)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
        }

        /// <summary>
        /// Gets the 'appearance' attribute values.
        /// </summary>
        public string Appearance
        {
            get { return GetAttributeValue("appearance"); }
        }

        /// <summary>
        /// Gets the 'value' attribute values.
        /// </summary>
        public string Value
        {
            get { return GetAttributeValue("value"); }
        }

        /// <summary>
        /// Gets the 'mediatype' attribute values.
        /// </summary>
        public string MediaType
        {
            get { return GetAttributeValue("mediatype"); }
        }
    }

}