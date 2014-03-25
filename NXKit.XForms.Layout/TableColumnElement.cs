using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    [Element("table-column")]
    public class TableColumnElement : 
        LayoutElement
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public TableColumnElement(XElement element)
            : base(element)
        {

        }

        public Importance Importance
        {
            get { return LayoutHelper.GetImportance(this); }
        }

    }

}
