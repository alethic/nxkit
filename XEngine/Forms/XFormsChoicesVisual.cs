using System.Xml.Linq;

namespace XEngine.Forms
{

    [VisualTypeDescriptor(Constants.XForms_1_0_NS, "choices")]
    public class XFormsChoicesVisualTypeDescriptor : VisualTypeDescriptor
    {

        public override Visual CreateVisual(IFormProcessor form, StructuralVisual parent, XNode node)
        {
            return new XFormsChoicesVisual(parent, (XElement)node);
        }

    }

    public class XFormsChoicesVisual : XFormsVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        public XFormsChoicesVisual(StructuralVisual parent, XElement element)
            : base(parent, element)
        {

        }

    }

}
