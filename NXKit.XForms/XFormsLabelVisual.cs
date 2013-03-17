using System.Collections.Generic;
using System.Xml.Linq;

namespace NXKit.XForms
{
    
    [VisualTypeDescriptor(Constants.XForms_1_0_NS, "label")]
    public class XFormsLabelVisualTypeDescriptor : VisualTypeDescriptor
    {

        public override Visual CreateVisual(IEngine form, StructuralVisual parent, XNode node)
        {
            return new XFormsLabelVisual(parent, (XElement)node);
        }

    }

    public class XFormsLabelVisual : XFormsSingleNodeBindingVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        public XFormsLabelVisual(StructuralVisual parent, XElement element)
            : base(parent, element)
        {

        }

        protected override IEnumerable<Visual> CreateChildren()
        {
            return CreateElementChildren(Element, includeTextContent: true);
        }

    }

}
