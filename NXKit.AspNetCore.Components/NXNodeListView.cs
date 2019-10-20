using System.Collections.Generic;
using System.Xml.Linq;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

using NXKit.Xml;

namespace NXKit.AspNetCore.Components
{

    /// <summary>
    /// Renders a set of NX nodes.
    /// </summary>
    public class NXNodeListView : ComponentBase
    {

        /// <summary>
        /// Elements to be rendered.
        /// </summary>
        [Parameter]
        public IEnumerable<XNode> Nodes { get; set; }

        /// <summary>
        /// Renders the element.
        /// </summary>
        /// <param name="builder"></param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            foreach (var node in Nodes)
            {
                var seq = node.GetObjectId() * 2;
                builder.OpenComponent<NXNodeView>(seq);
                builder.AddAttribute(seq + 1, nameof(NXNodeView.Node), node);
                builder.CloseComponent();
            }
        }

    }

}