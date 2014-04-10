using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    [Interface("{http://schemas.nxkit.org/2014/xforms-layout}table-row")]
    public class TableRow
    {

        readonly XElement element;
        readonly TableRowAttributes attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public TableRow(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
            this.attributes = new TableRowAttributes(element);
        }

        public string ColumnGroup
        {
            get { return attributes.ColumnGroup; }
        }

    }

}
