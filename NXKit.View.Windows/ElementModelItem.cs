using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.View.Windows
{

    public class ElementModelItem
    {

        readonly XElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public ElementModelItem(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
        }

        /// <summary>
        /// Gets the referenced element.
        /// </summary>
        public XElement Element
        {
            get { return element; }
        }

        public override string ToString()
        {
            return string.Format("[{0}]", element.Name);
        }

    }

}
