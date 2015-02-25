using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Xml.Linq;

using NXKit.View.Js;

namespace NXKit.Html.View.Js
{

    [Export(typeof(IViewModuleDependencyProvider))]
    public class ViewModuleDependencyProvider :
        IViewModuleDependencyProvider
    {

        static readonly ViewModuleDependency[] DEPENDENCIES = new[] 
        {
            new ViewModuleDependency(ViewModuleType.Script, "nxkit-html"),
            new ViewModuleDependency(ViewModuleType.Css, "nxkit-html.css"),
            new ViewModuleDependency(ViewModuleType.Template, "nxkit-html.html"),
        };

        public IEnumerable<ViewModuleDependency> GetDependencies(XObject obj)
        {
            var element = obj as XElement;

            if (element != null &&
                element.Name.Namespace == "http://www.w3.org/1999/xhtml")
                return DEPENDENCIES;

            return Enumerable.Empty<ViewModuleDependency>();
        }

    }

}
