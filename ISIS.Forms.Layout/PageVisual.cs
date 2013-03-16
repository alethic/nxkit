using System.Xml.Linq;

using ISIS.Forms.XForms;

namespace ISIS.Forms.Layout
{

    [VisualTypeDescriptor(Constants.Layout_1_0_NS, "page")]
    public class PageVisualTypeDescriptor : VisualTypeDescriptor
    {

        public override Visual CreateVisual(IFormProcessor form, StructuralVisual parent, XNode node)
        {
            return new PageVisual(parent, (XElement)node);
        }

    }

    public class PageVisual : XFormsGroupVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        public PageVisual(StructuralVisual parent, XElement element)
            : base(parent, element)
        {

        }

        public Importance Importance
        {
            get { return LayoutHelper.GetImportance(this); }
        }

    }

}
