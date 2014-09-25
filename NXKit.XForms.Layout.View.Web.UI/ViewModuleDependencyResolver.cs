using System.ComponentModel.Composition;

using NXKit.View.Js;
using NXKit.Web.UI;

namespace NXKit.Web.XForms.Layout.View.UI
{

    [Export(typeof(IViewModuleDependencyResolver))]
    public class ViewModuleDependencyResolver :
        IViewModuleDependencyResolver
    {

        public object Resolve(ViewModuleDependency dependency)
        {
            if (dependency.Type == ViewModuleType.Script &&
                dependency.Name == "nxkit-xforms-layout.js")
                return typeof(ViewModuleDependencyResolver).Assembly.GetManifestResourceStream("NXKit.Web.UI.Scripts.nxkit-xforms-layout.js");
            if (dependency.Type == ViewModuleType.Css &&
                dependency.Name == "nxkit-xforms-layout.css")
                return typeof(ViewModuleDependencyResolver).Assembly.GetManifestResourceStream("NXKit.Web.UI.Scripts.nxkit-xforms-layout.css");
            if (dependency.Type == ViewModuleType.View &&
                dependency.Name == "nxkit-xforms-layout.html")
                return typeof(ViewModuleDependencyResolver).Assembly.GetManifestResourceStream("NXKit.Web.UI.Scripts.nxkit-xforms-layout.html");

            return null;
        }

    }

}
