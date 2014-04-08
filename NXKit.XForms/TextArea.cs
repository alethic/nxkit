using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.XForms
{

    [Interface("{http://www.w3.org/2002/xforms}textarea")]
    public class TextArea 
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public TextArea(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

    }

}
