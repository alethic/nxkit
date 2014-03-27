using System.Xml.Linq;

namespace NXKit.XForms
{

    [Element("alert")]
    public class Alert :
        SingleNodeUIBindingElement,
        ISupportsCommonAttributes
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public Alert(XElement xml)
            : base(xml)
        {

        }

    }

}
