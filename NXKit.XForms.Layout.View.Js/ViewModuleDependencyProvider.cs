using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Xml.Linq;

using NXKit.View.Js;

namespace NXKit.XForms.Layout.View.Js
{

    [Export(typeof(IViewModuleDependencyProvider))]
    public class ViewModuleDependencyProvider :
        IViewModuleDependencyProvider
    {

        public IEnumerable<ViewModuleDependency> GetDependencies(XElement element)
        {
            if (element.Name.Namespace == Constants.Layout_1_0)
            {
                yield return new ViewModuleDependency(ViewModuleType.Script, "nxkit-xforms-layout.js");
                yield return new ViewModuleDependency(ViewModuleType.Css, "nxkit-xforms-layout.css");
                yield return new ViewModuleDependency(ViewModuleType.View, "nxkit-xforms-layout.html");
            }
        }

    }

}
