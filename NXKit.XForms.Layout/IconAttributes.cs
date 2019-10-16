using System;
using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    /// <summary>
    /// Makes the 'icon' attribute available.
    /// </summary>
    [Extension(typeof(IconAttributes), "{http://schemas.nxkit.org/2014/xforms-layout}icon")]
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
            if (element == null)
                throw new ArgumentNullException(nameof(element));
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
