using System.Collections.Generic;
using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    [Visual("p")]
    public class ParagraphVisual : LayoutVisual
    {

        protected override IEnumerable<Visual> CreateVisuals()
        {
            // preserve text nodes
            return CreateElementVisuals(Element, true);
        }

        public Importance Importance
        {
            get { return LayoutHelper.GetImportance(this); }
        }

    }

}
