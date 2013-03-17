using System.Xml.Linq;


namespace NXKit.XForms
{

    [VisualTypeDescriptor(Constants.XForms_1_0_NS, "select")]
    public class XFormsSelectVisualTypeDescriptor : VisualTypeDescriptor
    {

        public override Visual CreateVisual(IFormProcessor form, StructuralVisual parent, XNode node)
        {
            return new XFormsSelectVisual(parent, (XElement)node);
        }

    }

    public class XFormsSelectVisual : XFormsSingleNodeBindingVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        public XFormsSelectVisual(StructuralVisual parent, XElement element)
            : base(parent, element)
        {

        }

        public bool Incremental
        {
            get { return Module.GetAttributeValue(Element, "incremental") == "true"; }
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
