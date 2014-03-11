using System.Xml.Linq;

namespace NXKit.XForms
{

    [Visual("select")]
    public class XFormsSelectVisual : XFormsSingleNodeBindingVisual
    {

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

                var nc = new XFormsXsltContext(this);
                var st = appearanceAttr.Split(':');
                var ns = st.Length == 2 ? nc.LookupNamespace(st[0]) : null;
                var lp = st.Length == 2 ? st[1] : st[0];
                return XName.Get(lp, ns);
            }
        }

    }

}
