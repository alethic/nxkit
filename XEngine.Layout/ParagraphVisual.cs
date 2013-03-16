using System.Collections.Generic;
using System.Xml.Linq;

namespace XEngine.Forms.Layout
{

    [VisualTypeDescriptor(Constants.Layout_1_0_NS, "p")]
    public class ParagraphVisualTypeDescriptor : VisualTypeDescriptor
    {

        public override Visual CreateVisual(IFormProcessor form, StructuralVisual parent, XNode node)
        {
            return new ParagraphVisual(parent, (XElement)node);
        }

    }

    public class ParagraphVisual : LayoutVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        public ParagraphVisual(StructuralVisual parent, XElement element)
            : base(parent, element)
        {

        }

        protected override IEnumerable<Visual> CreateChildren()
        {
            // preserve text nodes
            return CreateElementChildren(Element, true);
        }

        public Importance Importance
        {
            get { return LayoutHelper.GetImportance(this); }
        }

    }

}
