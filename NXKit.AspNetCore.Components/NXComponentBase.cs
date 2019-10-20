using System.Xml.Linq;

using Microsoft.AspNetCore.Components;

namespace NXKit.AspNetCore.Components
{

    /// <summary>
    /// Base class to assist in implementing NX components.
    /// </summary>
    public abstract class NXComponentBase : ComponentBase, INXComponent
    {

        /// <summary>
        /// Gets a reference to the attached XML element.
        /// </summary>
        [Parameter]
        public XElement Element { get; set; }

    }

}
