using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    [Element("p")]
    public class Paragraph : 
        LayoutElement
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Paragraph(XElement element)
            : base(element)
        {

        }

    }

}
