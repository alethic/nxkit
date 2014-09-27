using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Xml.Linq;

using NXKit.View.Js;

namespace NXKit.XForms.View.Js
{

    [Export(typeof(IViewModuleDependencyProvider))]
    public class ViewModuleDependencyProvider :
        IViewModuleDependencyProvider
    {

        public IEnumerable<ViewModuleDependency> GetDependencies(XElement element)
        {
            if (element.Name.Namespace == Constants.XForms_1_0)
            {
                yield return new ViewModuleDependency(ViewModuleType.Script, "nxkit-xforms");
                yield return new ViewModuleDependency(ViewModuleType.Css, "nxkit-xforms.css");
                yield return new ViewModuleDependency(ViewModuleType.Template, "nxkit-xforms.html");
            }
        }

    }

}
