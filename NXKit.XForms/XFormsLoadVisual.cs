using System.Xml.Linq;

namespace NXKit.XForms
{

    [VisualTypeDescriptor(Constants.XForms_1_0_NS, "load")]
    public class XFormsLoadVisualTypeDescriptor : VisualTypeDescriptor
    {

        public override Visual CreateVisual(IEngine form, StructuralVisual parent, XNode node)
        {
            return new XFormsLoadVisual(parent, (XElement)node);
        }

    }

    public class XFormsLoadVisual : XFormsSingleNodeBindingVisual, IActionVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        internal XFormsLoadVisual(StructuralVisual parent, XElement element)
            : base(parent, element)
        {

        }

        public void Handle(Event ev)
        {
            Module.InvokeAction(this);
        }

        public void Invoke()
        {
            // ensure values are up to date
            Refresh();


        }

    }

}
