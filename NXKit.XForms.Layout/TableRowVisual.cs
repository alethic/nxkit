using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    [Visual( "table-row")]
    public class TableRowVisual : LayoutVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public TableRowVisual(XElement element)
            : base(element)
        {

        }


        public string ColumnGroup
        {
            get
            {
                return Module.GetAttributeValue(Xml, "column-group");
            }
        }

        public Importance Importance
        {
            get { return LayoutHelper.GetImportance(this); }
        }

    }

}
