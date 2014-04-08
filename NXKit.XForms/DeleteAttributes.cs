using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'delete' attributes.
    /// </summary>
    public class DeleteAttributes :
        CommonAttributes
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public DeleteAttributes(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

        /// <summary>
        /// Gets the 'at' attribute.
        /// </summary>
        public XAttribute AtAttribute
        {
            get { return GetAttribute("at"); }
        }

        /// <summary>
        /// Gets the 'at' attribute value.
        /// </summary>
        public string At
        {
            get { return GetAttributeValue("at"); }
        }

    }

}