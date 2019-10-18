using System.Collections.Generic;
using System.Xml.Linq;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

using NXKit.Xml;

namespace NXKit.View.Razor
{

    /// <summary>
    /// Renders a specific element.
    /// </summary>
    public class NXElementListView : ComponentBase
    {

        /// <summary>
        /// Elements to be rendered.
        /// </summary>
        [Parameter]
        public IEnumerable<XElement> Elements { get; set; }

        /// <summary>
        /// Renders the element.
        /// </summary>
        /// <param name="builder"></param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            foreach (var element in Elements)
            {
                var seq = element.GetObjectId() * 2;
                builder.OpenComponent<NXElementView>(seq);
                builder.AddAttribute(seq + 1, nameof(NXElementView.Element), element);
                builder.CloseComponent();
            }
        }

    }

}