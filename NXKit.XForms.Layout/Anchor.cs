using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    [Interface("{http://schemas.nxkit.org/2014/xforms-layout}a")]
    public class Anchor
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Anchor(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

    }

}
