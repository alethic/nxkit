using System.Xml.Linq;
using NXKit.Util;

namespace NXKit.XForms.Layout
{

    [Element("table-column-group")]
    public class TableColumnGroupElement :
        LayoutElement,
        ITableColumnGroupContainer
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public TableColumnGroupElement(XElement xml)
            : base(xml)
        {

        }

        public string Name
        {
            get { return Module.GetAttributeValue(Xml, "name").TrimToNull(); }
        }

    }

}
