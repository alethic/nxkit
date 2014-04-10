using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    [Interface("{http://schemas.nxkit.org/2014/xforms-layout}table-column")]
    public class TableColumn
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public TableColumn(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

    }

}
