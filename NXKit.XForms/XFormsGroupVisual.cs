using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace NXKit.XForms
{

    [Visual("group")]
    public class XFormsGroupVisual : XFormsSingleNodeBindingVisual, IRelevancyScope, INavigationCategoryVisual
    {

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

                var nc = new XFormsXsltContext(this);
                var st = appearanceAttr.Split(':');
                var ns = st.Length == 2 ? nc.LookupNamespace(st[0]) : null;
                var lp = st.Length == 2 ? st[1] : st[0];
                return XName.Get(lp, ns);
            }
        }

        public string Label
        {
            get { return GetLabel(); }
        }

        string GetLabel()
        {
            var b = new StringWriter();
            var l = Children.OfType<XFormsLabelVisual>().FirstOrDefault();
            if (l != null)
                l.WriteText(b);
            else
                return null;

            return b.ToString();
        }

    }

}
