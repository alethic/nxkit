using System;
using System.Xml.Linq;

namespace NXKit.XInclude
{

    /// <summary>
    /// Base class for an interface which provides XForms attributes of a specific element.
    /// </summary>
    public abstract class AttributeAccessor :
        NXKit.AttributeAccessor
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="defaultNamespace"></param>
        public AttributeAccessor(
            XElement element,
            XNamespace defaultNamespace)
            : base(element, defaultNamespace)
        {
            if (element is null)
                throw new ArgumentNullException(nameof(element));
            if (defaultNamespace is null)
                throw new ArgumentNullException(nameof(defaultNamespace));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public AttributeAccessor(XElement element)
            : this(element, "http://www.w3.org/2001/XInclude")
        {
            if (element is null)
                throw new ArgumentNullException(nameof(element));
        }

    }

}
