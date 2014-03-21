using System;
using System.Xml.Linq;

namespace NXKit.XForms.Layout
{

    /// <summary>
    /// Exposes the given <see cref="Visual"/> type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    internal class VisualAttribute : Attribute
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="namespace"></param>
        /// <param name="name"></param>
        public VisualAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets the name of <see cref="XElement"/>s handled by the decorated see <see cref="Visual"/>.
        /// </summary>
        public string Name { get; private set; }

    }

}
