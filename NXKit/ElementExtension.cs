using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Base class for an extension on <see cref="XElement"/> instances.
    /// </summary>
    public abstract class ElementExtension :
        NodeExtension
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public ElementExtension(XElement element)
            :base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

        /// <summary>
        /// Gets the <see cref="XElement"/> this extension applies to.
        /// </summary>
        public XElement Element
        {
            get { return (XElement)Node; }
        }

    }

}
