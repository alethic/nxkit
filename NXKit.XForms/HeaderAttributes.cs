using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the attributes for the 'header' element.
    /// </summary>
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
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentException>(element.Name == Constants.XForms_1_0 + "header");
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
