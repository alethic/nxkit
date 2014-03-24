using System.Xml.Linq;
using NXKit.Util;

namespace NXKit.XForms.Layout
{

    [Visual("table-column-group")]
    public class TableColumnGroupVisual :
        LayoutVisual,
        ITableColumnGroupContainer
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public TableColumnGroupVisual(XElement element)
            : base(element)
        {

        }

        public string Name
        {
            get { return Module.GetAttributeValue(Xml, "name").TrimToNull(); }
        }

    }

}
