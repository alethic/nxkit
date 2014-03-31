using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    [Element("table-column")]
    public class TableColumn : 
        LayoutElement
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public TableColumn(XElement element)
            : base(element)
        {

        }

        public Importance Importance
        {
            get { return LayoutHelper.GetImportance(this); }
        }

    }

}
