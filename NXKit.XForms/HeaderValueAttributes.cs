using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'value' element attributes.
    /// </summary>
    public class HeaderValueAttributes :
        AttributeAccessor
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public HeaderValueAttributes(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentException>(element.Name == Constants.XForms_1_0 + "value");
        }

        /// <summary>
        /// Gets the 'value' attribute value.
        /// </summary>
        public XAttribute Value
        {
            get { return GetAttribute("value"); }
        }

    }

}