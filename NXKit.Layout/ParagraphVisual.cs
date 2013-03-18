using System.Collections.Generic;
using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    [Visual("p")]
    public class ParagraphVisual : LayoutVisual
    {

        protected override IEnumerable<Visual> CreateChildren()
        {
            // preserve text nodes
            return CreateElementChildren(Element, true);
        }

        public Importance Importance
        {
            get { return LayoutHelper.GetImportance(this); }
        }

    }

}
