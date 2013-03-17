using System.Xml.Linq;

namespace NXKit.XForms
{

    [VisualTypeDescriptor(Constants.XForms_1_0_NS, "trigger")]
    public class XFormsActionTriggerTypeDescriptor : VisualTypeDescriptor
    {

        public override Visual CreateVisual(IEngine form, StructuralVisual parent, XNode node)
        {
            return new XFormsTriggerVisual(parent, (XElement)node);
        }

    }

    public class XFormsTriggerVisual : XFormsSingleNodeBindingVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        public XFormsTriggerVisual(StructuralVisual parent, XElement element)
            : base(parent, element)
        {

        }

    }

}
