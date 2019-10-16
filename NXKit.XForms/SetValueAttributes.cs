using System;
using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'setvalue' attributes.
    /// </summary>
    [Extension(typeof(SetValueAttributes), "{http://www.w3.org/2002/xforms}setvalue")]
    public class SetValueAttributes :
        CommonAttributes
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public SetValueAttributes(XElement element)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
        }

        /// <summary>
        /// Gets the 'value' attribute value.
        /// </summary>
        public string Value
        {
            get { return GetAttributeValue("value"); }
        }

    }

}