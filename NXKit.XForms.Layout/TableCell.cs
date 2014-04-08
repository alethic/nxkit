using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    [NXElementInterface("{http://schemas.nxkit.org/nxkit/2014/xforms-layout}table-cell")]
    public class TableCell
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public TableCell(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

    }

}
