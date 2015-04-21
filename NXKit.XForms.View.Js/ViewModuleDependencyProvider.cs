using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Xml.Linq;

using NXKit.View.Js;

namespace NXKit.XForms.View.Js
{

    [Export(typeof(IViewModuleDependencyProvider))]
    public class ViewModuleDependencyProvider :
        IViewModuleDependencyProvider
    {

        static readonly ViewModuleDependency[] DEPENDENCIES = new[]
        {
            new ViewModuleDependency(ViewModuleType.Script, "nxkit-xforms"),
            new ViewModuleDependency(ViewModuleType.Css, "nxkit-xforms.css"),
            new ViewModuleDependency(ViewModuleType.Template, "nxkit-xforms.html"),
            new ViewModuleDependency(ViewModuleType.Script, "nx-moment"),
        };

        public IEnumerable<ViewModuleDependency> GetDependencies(XObject obj)
        {
            var element = obj as XElement;

            if (element != null &&
                element.Name.Namespace == Constants.XForms_1_0)
                return DEPENDENCIES;

            return Enumerable.Empty<ViewModuleDependency>();
        }

    }

}
