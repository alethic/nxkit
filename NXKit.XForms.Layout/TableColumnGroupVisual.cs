using NXKit.Util;

namespace NXKit.XForms.Layout
{

    [Visual("table-column-group")]
    public class TableColumnGroupVisual :
        LayoutVisual,
        ITableColumnGroupContainer
    {

        public string Name
        {
            get { return Module.GetAttributeValue(Element, "name").TrimToNull(); }
        }

    }

}
