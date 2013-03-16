using System.Xml.Linq;

namespace ISIS.Forms.XForms
{
    
    [VisualTypeDescriptor(Constants.XForms_1_0_NS, "input")]
    public class XFormsInputVisualTypeDescriptor : VisualTypeDescriptor
    {

        public override Visual CreateVisual(IFormProcessor form, StructuralVisual parent, XNode node)
        {
            return new XFormsInputVisual(parent, (XElement)node);
        }

    }

    public class XFormsInputVisual : XFormsSingleNodeBindingVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        public XFormsInputVisual(StructuralVisual parent, XElement element)
            : base(parent, element)
        {

        }

        public bool Incremental
        {
            get { return Module.GetAttributeValue(Element, "incremental") == "true"; }
        }

    }

}
