using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    [Visual( "table-row")]
    public class TableRowVisual : LayoutVisual
    {

        public string ColumnGroup
        {
            get
            {
                return Module.GetAttributeValue(Element, "column-group");
            }
        }

        public Importance Importance
        {
            get { return LayoutHelper.GetImportance(this); }
        }

    }

}
