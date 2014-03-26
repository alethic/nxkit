using System.Xml.Linq;

namespace NXKit.XForms
{

    [Element("trigger")]
    public class TriggerElement :
        SingleNodeUIBindingElement
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public TriggerElement(XElement element)
            : base(element)
        {

        }

    }

}
