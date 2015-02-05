using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Xml.Linq;
using NXKit.View.Js;

namespace NXKit.XForms.Layout.View.Js
{

    [Export(typeof(IViewModuleDependencyProvider))]
    public class ViewModuleDependencyProvider :
        IViewModuleDependencyProvider
    {

        static readonly ViewModuleDependency[] DEPENDENCIES = new[] 
        {
            new ViewModuleDependency(ViewModuleType.Script, "nxkit-xforms-layout"),
            new ViewModuleDependency(ViewModuleType.Css, "nxkit-xforms-layout.css"),
            new ViewModuleDependency(ViewModuleType.Template, "nxkit-xforms-layout.html"),
        };

        public IEnumerable<ViewModuleDependency> GetDependencies(XObject obj)
        {
            var element = obj as XElement;

            if (element != null &&
                element.Name.Namespace == Constants.Layout_1_0)
                return DEPENDENCIES;

            return Enumerable.Empty<ViewModuleDependency>();
        }

    }

}
