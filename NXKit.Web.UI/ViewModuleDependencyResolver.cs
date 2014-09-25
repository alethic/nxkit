using System.ComponentModel.Composition;

using NXKit.View.Js;

namespace NXKit.Web.UI
{

    [Export(typeof(IViewModuleDependencyResolver))]
    public class ViewModuleDependencyResolver :
        IViewModuleDependencyResolver
    {

        public object Resolve(ViewModuleDependency dependency)
        {
            if (dependency.Type == ViewModuleType.Script &&
                dependency.Name == "nxkit.js")
                return typeof(ViewModuleDependencyResolver).Assembly.GetManifestResourceStream("NXKit.Web.UI.Scripts.nxkit.js");
            if (dependency.Type == ViewModuleType.Css &&
                dependency.Name == "nxkit.css")
                return typeof(ViewModuleDependencyResolver).Assembly.GetManifestResourceStream("NXKit.Web.UI.Scripts.nxkit.css");
            if (dependency.Type == ViewModuleType.View &&
                dependency.Name == "nxkit.html")
                return typeof(ViewModuleDependencyResolver).Assembly.GetManifestResourceStream("NXKit.Web.UI.Scripts.nxkit.html");

            return null;
        }

    }

}
