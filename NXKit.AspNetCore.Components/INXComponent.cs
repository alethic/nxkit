using System.Xml.Linq;

using Microsoft.AspNetCore.Components;

namespace NXKit.AspNetCore.Components
{

    /// <summary>
    /// Describes a UI component attached to an NX document element.
    /// </summary>
    public interface INXComponent : IComponent
    {

        /// <summary>
        /// Gets the attached NX element.
        /// </summary>
        [Parameter]
        XElement Element { get; set; }

    }

}
