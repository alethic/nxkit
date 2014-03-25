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
        /// <param name="element"></param>
        public TableColumnGroupElement(XElement element)
            : base(element)
        {

        }

        public string Name
        {
            get { return Module.GetAttributeValue(Xml, "name").TrimToNull(); }
        }

    }

}
