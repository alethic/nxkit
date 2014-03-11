using System.Collections.Generic;
using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    [Visual("a")]
    public class AnchorVisual : LayoutVisual
    {

        protected override IEnumerable<Visual> CreateChildren()
        {
            // preserve text nodes
            return CreateElementChildren(Element, true);
        }

        public string Href
        {
            get { return Document.GetModule<LayoutModule>().GetAttributeValue(Element, "href"); }
        }

    }

}
