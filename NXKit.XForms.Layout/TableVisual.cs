using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    [Visual("table")]
    public class TableVisual : 
        XFormsGroupVisual,
        ITableColumnGroupContainer
    {
        
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public TableVisual(XElement xml)
            : base(xml)
        {

        }

        public Importance Importance
        {
            get { return LayoutHelper.GetImportance(this); }
        }

    }

}
