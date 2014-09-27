using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Xml.Linq;

namespace NXKit.View.Js
{

    [Export(typeof(IViewModuleDependencyProvider))]
    public class ViewModuleDependencyProvider :
        IViewModuleDependencyProvider
    {

        public IEnumerable<ViewModuleDependency> GetDependencies(XElement element)
        {
            yield return new ViewModuleDependency(ViewModuleType.Script, "nxkit");
            yield return new ViewModuleDependency(ViewModuleType.Css, "nxkit.css");
            yield return new ViewModuleDependency(ViewModuleType.Template, "nxkit.html");
        }

    }

}
