using System.Collections.Generic;
using System.Xml.Linq;

namespace ISIS.Forms.XForms
{

    [VisualTypeDescriptor(Constants.XForms_1_0_NS, "alert")]
    public class XFormsAlertVisualTypeDescriptor : VisualTypeDescriptor
    {

        public override Visual CreateVisual(IFormProcessor form, StructuralVisual parent, XNode node)
        {
            return new XFormsAlertVisual(parent, (XElement)node);
        }

    }

    public class XFormsAlertVisual : XFormsSingleNodeBindingVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        public XFormsAlertVisual(StructuralVisual parent, XElement element)
            : base(parent, element)
        {

        }

        protected override IEnumerable<Visual> CreateChildren()
        {
            return CreateElementChildren(Element, includeTextContent: true);
        }

    }

}
