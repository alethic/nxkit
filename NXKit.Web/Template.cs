using System;
using System.Diagnostics.Contracts;

namespace NXKit.Web
{

    public abstract class Template :
        ITemplate
    {

        readonly NXElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Template(NXElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
        }

        /// <summary>
        /// Gets the element for this template interface.
        /// </summary>
        public NXElement Element
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
