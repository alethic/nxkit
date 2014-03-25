using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    [Element("p")]
    public class ParagraphElement : 
        LayoutElement
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public ParagraphElement(XElement element)
            : base(element)
        {

        }

        public Importance Importance
        {
            get { return LayoutHelper.GetImportance(this); }
        }

    }

}
