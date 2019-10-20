using System;
using System.Linq;
using System.Xml.Linq;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

using NXKit.Xml;

namespace NXKit.AspNetCore.Components
{

    /// <summary>
    /// Renders a specific NX element.
    /// </summary>
    public class NXNodeView : ComponentBase
    {

        /// <summary>
        /// Node to be rendered.
        /// </summary>
        [Parameter]
        public XNode Node { get; set; }

        /// <summary>
        /// Renders the node.
        /// </summary>
        /// <param name="builder"></param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (Node != null)
            {
                switch (Node)
                {
                    case XText text:
                        builder.OpenComponent(0, typeof(NXText));
                        builder.AddAttribute(1, nameof(NXText.Text), text);
                        builder.CloseComponent();
                        break;
                    case XElement element:
                        builder.OpenComponent(0, GetComponentType(element));
                        builder.AddAttribute(1, nameof(INXComponent.Element), element);
                        builder.CloseComponent();
                        break;
                }
            }
        }

        Type GetComponentType(XElement element)
        {
            return Node.Interfaces<INXComponentTypeProvider>().Select(i => i.GetComponentType(element)).FirstOrDefault(i => i != null) ?? typeof(NXUnknown);
        }

    }

}
