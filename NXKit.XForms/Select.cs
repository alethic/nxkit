using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.XForms
{

    [Interface("{http://www.w3.org/2002/xforms}select")]
    public class Select
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Select(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

    }

}
