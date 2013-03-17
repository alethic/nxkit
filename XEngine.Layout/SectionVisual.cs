using System.Xml.Linq;

using NXKit.XForms;

namespace NXKit.Layout
{

    [VisualTypeDescriptor(Constants.Layout_1_0_NS, "section")]
    public class SectionVisualTypeDescriptor : VisualTypeDescriptor
    {

        public override Visual CreateVisual(IFormProcessor form, StructuralVisual parent, XNode element)
        {
            return new SectionVisual(parent, (XElement)element);
        }

    }

    public class SectionVisual : XFormsGroupVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        public SectionVisual(StructuralVisual parent, XElement element)
            : base(parent, element)
        {

        }

    }

}
