using System.Xml.Linq;

namespace NXKit.Layout
{

    [VisualTypeDescriptor(Constants.Layout_1_0_NS, "table-row")]
    public class TableRowVisualTypeDescriptor : VisualTypeDescriptor
    {

        public override Visual CreateVisual(IEngine form, StructuralVisual parent, XNode element)
        {
            return new TableRowVisual(parent, (XElement)element);
        }

    }

    public class TableRowVisual : LayoutVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        public TableRowVisual(StructuralVisual parent, XElement element)
            : base(parent, element)
        {

        }

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
