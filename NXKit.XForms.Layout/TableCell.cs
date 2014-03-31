using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    [Element("table-cell")]
    public class TableCell : 
        LayoutElement
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public TableCell(XElement element)
            : base(element)
        {

        }

        public Importance Importance
        {
            get { return LayoutHelper.GetImportance(this); }
        }

    }

}
