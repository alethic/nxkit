using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    [Element( "table-row")]
    public class TableRow : LayoutElement
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public TableRow(XElement element)
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
