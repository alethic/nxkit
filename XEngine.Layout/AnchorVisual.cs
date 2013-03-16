using System.Collections.Generic;
using System.Xml.Linq;

namespace XEngine.Forms.Layout
{

    [VisualTypeDescriptor(Constants.Layout_1_0_NS, "a")]
    public class AnchorVisualTypeDescriptor : VisualTypeDescriptor
    {

        public override Visual CreateVisual(IFormProcessor form, StructuralVisual parent, XNode element)
        {
            return new AnchorVisual(parent, (XElement)element);
        }

    }

    public class AnchorVisual : LayoutVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        public AnchorVisual(StructuralVisual parent, XElement element)
            : base(parent, element)
        {

        }

        protected override IEnumerable<Visual> CreateChildren()
        {
            // preserve text nodes
            return CreateElementChildren(Element, true);
        }

        public string Href
        {
            get { return Form.GetModule<LayoutModule>().GetAttributeValue(Element, "href"); }
        }

    }

}
