using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace NXKit.XForms
{

    [Element("alert")]
    public class AlertElement :
        SingleNodeUIBindingElement
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public AlertElement(XElement element)
            : base(element)
        {

        }

    }

}
