using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.Imaging
{

    /// <summary>
    /// Base class for an interface which provides XForms.Imagine attributes of a specific element.
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
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(defaultNamespace != null);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public AttributeAccessor(XElement element)
            : this(element, "http://schemas.nxkit.org/2015/imaging")
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

    }

}
