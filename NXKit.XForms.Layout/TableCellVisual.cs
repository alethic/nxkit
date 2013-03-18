using System.Collections.Generic;

namespace NXKit.XForms.Layout
{

    [Visual("table-cell")]
    public class TableCellVisual : LayoutVisual
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
