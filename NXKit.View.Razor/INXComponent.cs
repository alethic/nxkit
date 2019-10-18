using System.Xml.Linq;

using Microsoft.AspNetCore.Components;

namespace NXKit.View.Razor
{

    public interface INXComponent
    {

        [Parameter]
        XElement Element { get; set; }

    }

}
