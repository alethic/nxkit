using System.Xml.Linq;
using NXKit.Util;

namespace NXKit.XForms.Layout
{

    [Element("table-column-group")]
    public class TableColumnGroup :
        LayoutElement,
        ITableColumnGroupContainer
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public TableColumnGroup(XElement xml)
            : base(xml)
        {

        }

        public new string Name
        {
            get { return Module.GetAttributeValue(Xml, "name").TrimToNull(); }
        }

    }

}
