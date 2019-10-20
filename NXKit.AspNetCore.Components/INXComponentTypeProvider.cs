using System;
using System.Xml.Linq;

namespace NXKit.AspNetCore.Components
{

    /// <summary>
    /// Provides an interface for generating Razor components for a given element.
    /// </summary>
    public interface INXComponentTypeProvider : IElementExtension
    {

        /// <summary>
        /// Returns the component type to render the specified element.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        Type GetComponentType(XElement element);

    }

}
