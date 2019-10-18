using System.Xml.Linq;

using Microsoft.AspNetCore.Components;

namespace NXKit.View.Razor
{

    public abstract class NXComponentBase : ComponentBase, INXComponent
    {

        [Parameter]
        public XElement Element { get; set; }

    }

}
