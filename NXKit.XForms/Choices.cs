using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.XForms
{

    [Interface("{http://www.w3.org/2002/xforms}choices")]
    public class Choices
    {

        readonly XElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Choices(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
        }

        /// <summary>
        /// Gets the <see cref="XElement"/>.
        /// </summary>
        public XElement Element
        {
            get { return element; }
        }

    }

}
