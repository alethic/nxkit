using System.Xml.Linq;

using XEngine.Forms;

namespace XEngine.Layout
{

    [VisualTypeDescriptor(Constants.Layout_1_0_NS, "category")]
    public class CategoryVisualTypeDescriptor : VisualTypeDescriptor
    {

        public override Visual CreateVisual(IFormProcessor form, StructuralVisual parent, XNode node)
        {
            return new CategoryVisual(parent, (XElement)node);
        }

    }

    public class CategoryVisual : XFormsGroupVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        public CategoryVisual(StructuralVisual parent, XElement element)
            : base(parent, element)
        {

        }

    }

}
