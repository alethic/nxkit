using System.Xml.Linq;
namespace NXKit.XForms.Layout
{

    [Element( "page")]
    public class PageElement :
        GroupElement
    {
        
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public PageElement(XElement xml)
            : base(xml)
        {

        }

        public Importance Importance
        {
            get { return LayoutHelper.GetImportance(this); }
        }

    }

}
