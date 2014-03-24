using System.Xml.Linq;
namespace NXKit.XForms.Layout
{

    [Visual( "page")]
    public class PageVisual :
        XFormsGroupVisual
    {
        
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public PageVisual(XElement xml)
            : base(xml)
        {

        }

        public Importance Importance
        {
            get { return LayoutHelper.GetImportance(this); }
        }

    }

}
