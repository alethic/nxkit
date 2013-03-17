using System;
using System.Xml.Linq;

namespace NXKit.XForms
{

    [VisualTypeDescriptor(Constants.XForms_1_0_NS, "dispatch")]
    public class XFormsDispatchVisualTypeDescriptor : VisualTypeDescriptor
    {

        public override Visual CreateVisual(IEngine form, StructuralVisual parent, XNode node)
        {
            return new XFormsDispatchVisual(parent, (XElement)node);
        }

    }

    public class XFormsDispatchVisual : XFormsVisual, IActionVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        internal XFormsDispatchVisual(StructuralVisual parent, XElement element)
            : base(parent, element)
        {

        }

        public void Handle(Event ev)
        {
            Invoke();
        }

        public void Invoke()
        {
            throw new NotImplementedException();
        }

    }

}
