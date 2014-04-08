using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    [Interface("{http://schemas.nxkit.org/nxkit/2014/xforms-layout}table-column-group")]
    public class TableColumnGroup :
        ITableColumnGroupContainer
    {

        readonly XElement element;
        readonly TableColumnGroupAttributes attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public TableColumnGroup(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
            this.attributes = new TableColumnGroupAttributes(element);
        }

        public string Name
        {
            get { return attributes.Name; }
        }

    }

}
