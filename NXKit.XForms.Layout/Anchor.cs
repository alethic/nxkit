using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    [Element("a")]
    public class Anchor : 
        LayoutElement
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Anchor(XElement element)
            : base(element)
        {

        }

        public string Href
        {
            get { return Document.Module<LayoutModule>().GetAttributeValue(Xml, "href"); }
        }

    }

}
