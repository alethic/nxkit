using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Abstract base <see cref="NXNode"/> implementation for node-set binding elements.
    /// </summary>
    public abstract class NodeSetBindingElement :
        UIBindingElement
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public NodeSetBindingElement(XElement xml)
            : base(xml)
        {

        }

    }

}
