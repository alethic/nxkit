using System.Linq;

using Microsoft.AspNetCore.Components.Rendering;

namespace NXKit.AspNetCore.Components.XHtml
{

    /// <summary>
    /// Renders a standard HTML component.
    /// </summary>
    public class GenericComponent : XHtmlComponentBase
    {

        /// <summary>
        /// Renders the node.
        /// </summary>
        /// <param name="builder"></param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, Element.Name.LocalName);
            builder.AddMultipleAttributes(1, Element.Attributes().ToDictionary(i => i.Name.LocalName, i => (object)i.Value));
            builder.OpenComponent<NXNodeListView>(3);
            builder.AddAttribute(4, "Nodes", Element.Nodes());
            builder.CloseComponent();
            builder.CloseElement();
        }

    }

}
