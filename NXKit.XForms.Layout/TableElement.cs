using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    [Element("table")]
    public class TableElement : 
        GroupElement,
        ITableColumnGroupContainer
    {
        
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public TableElement(XElement xml)
            : base(xml)
        {

        }

        public Importance Importance
        {
            get { return LayoutHelper.GetImportance(this); }
        }

    }

}
