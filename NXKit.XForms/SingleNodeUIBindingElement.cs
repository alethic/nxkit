using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Base implementation for an XForms element which implements Single-Node Binding.
    /// </summary>
    public class SingleNodeUIBindingElement :
        UIBindingElement
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public SingleNodeUIBindingElement(XName name)
            : base(name)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public SingleNodeUIBindingElement(XElement xml)
            : base(xml)
        {

        }

    }

}
