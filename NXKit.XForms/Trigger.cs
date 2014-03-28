using System.Xml.Linq;

namespace NXKit.XForms
{

    [Element("trigger")]
    public class Trigger :
        SingleNodeUIBindingElement
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public Trigger(XElement xml)
            : base(xml)
        {

        }

    }

}
