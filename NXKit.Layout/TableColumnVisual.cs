using System.Collections.Generic;
using System.Xml.Linq;

namespace NXKit.Layout
{

    [VisualTypeDescriptor(Constants.Layout_1_0_NS, "table-column")]
    public class TableColumnVisualTypeDescriptor : VisualTypeDescriptor
    {

        public override Visual CreateVisual(IEngine form, StructuralVisual parent, XNode node)
        {
            return new TableColumnVisual(parent, (XElement)node);
        }

    }

    public class TableColumnVisual : LayoutVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        public TableColumnVisual(StructuralVisual parent, XElement element)
            : base(parent, element)
        {

        }

        protected override IEnumerable<Visual> CreateChildren()
        {
            return CreateElementChildren(Element, true);
        }

        public Importance Importance
        {
            get { return LayoutHelper.GetImportance(this); }
        }

    }

}
