using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    [Visual("a")]
    public class AnchorVisual : 
        LayoutVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public AnchorVisual(XElement element)
            : base(element)
        {

        }

        public string Href
        {
            get { return Document.Module<LayoutModule>().GetAttributeValue(Xml, "href"); }
        }

    }

}
