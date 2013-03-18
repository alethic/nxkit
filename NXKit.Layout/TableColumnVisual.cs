using System.Collections.Generic;
using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    [Visual("table-column")]
    public class TableColumnVisual : LayoutVisual
    {

        protected override IEnumerable<Visual> CreateChildren()
        {
            return CreateElementChildren(Element, true);
        }

        public Importance Importance
        {
            get { return LayoutHelper.GetImportance(this); }
        }

    }

}
