using System.Collections.Generic;
using System.Xml.Linq;

namespace ISIS.Forms.XForms
{

    [VisualTypeDescriptor(Constants.XForms_1_0_NS, "group")]
    public class XFormsGroupVisualTypeDescriptor : VisualTypeDescriptor
    {

        public override Visual CreateVisual(IFormProcessor form, StructuralVisual parent, XNode node)
        {
            return new XFormsGroupVisual(parent, (XElement)node);
        }

    }

    public class XFormsGroupVisual : XFormsSingleNodeBindingVisual, IRelevancyScope
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        public XFormsGroupVisual(StructuralVisual parent, XElement element)
            : base(parent, element)
        {

        }

        protected override IEnumerable<Visual> CreateChildren()
        {
            return CreateElementChildren(Element);
        }

        public XName Appearance
        {
            get
            {
                var appearanceAttr = Module.GetAttributeValue(Element, "appearance");
                if (appearanceAttr == null)
                    return null;

                var nc = new VisualXmlNamespaceContext(this);
                var st = appearanceAttr.Split(':');
                var ns = st.Length == 2 ? nc.LookupNamespace(st[0]) : null;
                var lp = st.Length == 2 ? st[1] : st[0];
                return XName.Get(lp, ns);
            }
        }

    }

}
