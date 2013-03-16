using System.Xml.Linq;

namespace XEngine.Layout
{

    [VisualTypeDescriptor(Constants.Layout_1_0_NS, "table")]
    public class TableVisualTypeDescriptor : VisualTypeDescriptor
    {

        public override Visual CreateVisual(IFormProcessor form, StructuralVisual parent, XNode node)
        {
            return new TableVisual(parent, (XElement)node);
        }

    }

    public class TableVisual : LayoutVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        public TableVisual(StructuralVisual parent, XElement element)
            : base(parent, element)
        {

        }

        public Importance Importance
        {
            get { return LayoutHelper.GetImportance(this); }
        }

    }

}
