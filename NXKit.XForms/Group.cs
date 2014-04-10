using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.XForms
{

    [Interface("{http://www.w3.org/2002/xforms}group")]
    public class Group :
        ElementExtension
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public Group(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

    }

}
