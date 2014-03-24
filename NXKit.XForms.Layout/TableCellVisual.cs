using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    [Visual("table-cell")]
    public class TableCellVisual : 
        LayoutVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public TableCellVisual(XElement element)
            : base(element)
        {

        }

        public Importance Importance
        {
            get { return LayoutHelper.GetImportance(this); }
        }

    }

}
