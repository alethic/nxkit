using System.Collections.Generic;

namespace NXKit.XForms.Layout
{

    [Visual("table-cell")]
    public class TableCellVisual : LayoutVisual
    {

        protected override IEnumerable<Visual> CreateVisuals()
        {
            return CreateElementVisuals(Element, true);
        }

        public Importance Importance
        {
            get { return LayoutHelper.GetImportance(this); }
        }

    }

}
