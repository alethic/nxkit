using System.Xml.Linq;

namespace XEngine.Forms.XForms
{

    [VisualTypeDescriptor(Constants.XForms_1_0_NS, "output")]
    public class XFormsOutputVisualTypeDescriptor : VisualTypeDescriptor
    {

        public override Visual CreateVisual(IFormProcessor form, StructuralVisual parent, XNode node)
        {
            return new XFormsOutputVisual(parent, (XElement)node);
        }

    }

    public class XFormsOutputVisual : XFormsSingleNodeBindingVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        public XFormsOutputVisual(StructuralVisual parent, XElement element)
            : base(parent, element)
        {

        }

    }

}
