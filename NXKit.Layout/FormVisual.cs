using System.Xml.Linq;

namespace NXKit.Layout
{

    [VisualTypeDescriptor(Constants.Layout_1_0_NS, "form")]
    public class FormVisualTypeDescriptor : VisualTypeDescriptor
    {

        public override Visual CreateVisual(IEngine form, StructuralVisual parent, XNode node)
        {
            return new FormVisual(form, (XElement)node);
        }

    }

    public class FormVisual : LayoutVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="form"></param>
        /// <param name="element"></param>
        public FormVisual(IEngine form, XElement element)
            : base(form, null, element)
        {

        }

        public override string Id
        {
            get { return "FORM"; }
        }

    }

}
