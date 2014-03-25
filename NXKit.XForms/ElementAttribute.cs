using System;
using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Exposes the given <see cref="NXNode"/> type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    internal class ElementAttribute : Attribute
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="namespace"></param>
        /// <param name="name"></param>
        public ElementAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets the name of <see cref="XElement"/>s handled by the decorated see <see cref="NXNode"/>.
        /// </summary>
        public string Name { get; private set; }

    }

}
