using System;
using System.Xml.Linq;

namespace NXKit.XMLEvents
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
        public AttributeAccessor(XElement element, XNamespace defaultNamespace)
            : base(element, defaultNamespace)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            if (defaultNamespace == null)
                throw new ArgumentNullException(nameof(defaultNamespace));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public AttributeAccessor(XElement element)
            : this(element, SchemaConstants.Events_1_0)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
        }

    }

}
