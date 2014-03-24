using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    [Visual("p")]
    public class ParagraphVisual : 
        LayoutVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public ParagraphVisual(XElement element)
            : base(element)
        {

        }

        public Importance Importance
        {
            get { return LayoutHelper.GetImportance(this); }
        }

    }

}
