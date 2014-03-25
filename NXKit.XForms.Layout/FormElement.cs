using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    [Element("form")]
    public class FormElement : 
        LayoutElement
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public FormElement(XElement element)
            : base(element)
        {

        }

        public override string Id
        {
            get { return "FORM"; }
        }

    }

}
