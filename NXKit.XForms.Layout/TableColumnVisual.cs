using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    [Visual("table-column")]
    public class TableColumnVisual : 
        LayoutVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public TableColumnVisual(XElement element)
            : base(element)
        {

        }

        public Importance Importance
        {
            get { return LayoutHelper.GetImportance(this); }
        }

    }

}
