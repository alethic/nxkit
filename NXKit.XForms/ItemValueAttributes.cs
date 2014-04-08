using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'value' element attributes.
    /// </summary>
    public class ItemValueAttributes :
        AttributeAccessor
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public ItemValueAttributes(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

        /// <summary>
        /// Gets the 'value' attribute values.
        /// </summary>
        public string Value
        {
            get { return GetAttributeValue("value"); }
        }

    }

}