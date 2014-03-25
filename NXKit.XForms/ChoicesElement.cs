using System.Xml.Linq;

namespace NXKit.XForms
{

    [Element("choices")]
    public class ChoicesElement :
        XFormsElement
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public ChoicesElement(XElement xml)
            : base(xml)
        {

        }

    }

}
