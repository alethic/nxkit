using System.Xml.Linq;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

using NXKit.Xml;

namespace NXKit.View.Razor
{

    /// <summary>
    /// Renders a specific element.
    /// </summary>
    public class NXElementView : ComponentBase
    {

        /// <summary>
        /// Element to be rendered.
        /// </summary>
        [Parameter]
        public XElement Element { get; set; }

        /// <summary>
        /// Renders the element.
        /// </summary>
        /// <param name="builder"></param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (Element != null)
            {
                builder.OpenComponent(0, Element.InterfaceOrDefault<INXComponentTypeProvider>()?.GetComponentType(Element) ?? typeof(NXUnknown));
                builder.AddAttribute(1, nameof(INXComponent.Element), Element);
                builder.CloseComponent();
            }
        }

    }

}
