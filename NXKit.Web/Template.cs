using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.Web
{

    public abstract class Template :
        ITemplate
    {

        readonly XElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Template(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
        }

        /// <summary>
        /// Gets the element for this template interface.
        /// </summary>
        public XElement Element
        {
            get { return element; }
        }

        /// <summary>
        /// Gets the name of the template.
        /// </summary>
        public abstract string Name
        {
            get;
        }

    }

}
