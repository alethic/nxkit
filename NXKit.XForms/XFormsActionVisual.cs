using System.Linq;
using System.Xml.Linq;

namespace NXKit.XForms
{

    [VisualTypeDescriptor(Constants.XForms_1_0_NS, "action")]
    public class XFormsActionVisualTypeDescriptor : VisualTypeDescriptor
    {

        public override Visual CreateVisual(IEngine form, StructuralVisual parent, XNode node)
        {
            return new XFormsActionVisual(parent, (XElement)node);
        }

    }

    public class XFormsActionVisual : XFormsVisual, IActionVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        internal XFormsActionVisual(StructuralVisual parent, XElement element)
            : base(parent, element)
        {

        }

        public void Handle(Event ev)
        {
            Module.InvokeAction(this);
        }

        public void Invoke()
        {
            foreach (var action in Children.OfType<IActionVisual>())
                Module.InvokeAction(action);
        }

    }

}
