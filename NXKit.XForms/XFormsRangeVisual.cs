using System.Xml.Linq;

namespace NXKit.XForms
{

    [VisualTypeDescriptor(Constants.XForms_1_0_NS, "range")]
    public class XFormsRangeVisualTypeDescriptor : VisualTypeDescriptor
    {

        public override Visual CreateVisual(IEngine form, StructuralVisual parent, XNode node)
        {
            return new XFormsRangeVisual(parent, (XElement)node);
        }

    }

    public class XFormsRangeVisual : XFormsSingleNodeBindingVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        public XFormsRangeVisual(StructuralVisual parent, XElement element)
            : base(parent, element)
        {

        }

        public bool Incremental
        {
            get { return Module.GetAttributeValue(Element, "incremental") == "true"; }
        }

        public string Start
        {
            get { return Module.GetAttributeValue(Element, "start"); }
        }

        public string End
        {
            get { return Module.GetAttributeValue(Element, "end"); }
        }

        public string Step
        {
            get { return Module.GetAttributeValue(Element, "step"); }
        }

    }

}
