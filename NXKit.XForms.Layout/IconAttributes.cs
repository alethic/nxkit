using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    /// <summary>
    /// Makes the 'icon' attribute available.
    /// </summary>
    public class IconAttributes :
        AttributeAccessor
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public IconAttributes(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

        /// <summary>
        /// Gets the 'name' attribute value.
        /// </summary>
        public string Name
        {
            get { return GetAttributeValue("name"); }
        }

    }

}
