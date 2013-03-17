using System.Collections.Generic;
using System.Xml.Linq;

namespace NXKit.XForms
{

    [VisualTypeDescriptor(Constants.XForms_1_0_NS, "instance")]
    public class XFormsInstanceVisualTypeDescriptor : VisualTypeDescriptor
    {

        public override Visual CreateVisual(IFormProcessor form, StructuralVisual parent, XNode node)
        {
            return new XFormsInstanceVisual(parent, (XElement)node);
        }

    }

    public class XFormsInstanceVisual : XFormsVisual
    {

        private XFormsInstanceVisualState state;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        public XFormsInstanceVisual(StructuralVisual parent, XElement element)
            : base(parent, element)
        {

        }

        public XFormsInstanceVisualState State
        {
            get { return state ?? (state = GetState<XFormsInstanceVisualState>()); }
        }

        protected override IEnumerable<Visual> CreateChildren()
        {
            // an instance has no visual children
            yield break;
        }

    }

}
