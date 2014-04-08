using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.XForms
{

    [Interface("{http://www.w3.org/2002/xforms}group")]
    public class Group 
    {

        readonly XElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public Group(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
        }

    }

}
