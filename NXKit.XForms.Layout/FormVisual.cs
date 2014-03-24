using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    [Visual("form")]
    public class FormVisual : 
        LayoutVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public FormVisual(XElement element)
            : base(element)
        {

        }

        public override string Id
        {
            get { return "FORM"; }
        }

    }

}
