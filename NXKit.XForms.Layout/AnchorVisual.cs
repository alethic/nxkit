using System.Collections.Generic;
using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    [Visual("a")]
    public class AnchorVisual : LayoutVisual
    {

        protected override IEnumerable<Visual> CreateVisuals()
        {
            // preserve text nodes
            return CreateElementVisuals(Element, true);
        }

        public string Href
        {
            get { return Document.GetModule<LayoutModule>().GetAttributeValue(Element, "href"); }
        }

    }

}
