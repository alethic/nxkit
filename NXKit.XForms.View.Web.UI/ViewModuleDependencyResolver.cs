using System.ComponentModel.Composition;

using NXKit.View.Js;
using NXKit.Web.UI;

namespace NXKit.XForms.View.Web.UI
{

    [Export(typeof(IViewModuleDependencyResolver))]
    public class ViewModuleDependencyResolver :
        IViewModuleDependencyResolver
    {

        public object Resolve(ViewModuleDependency dependency)
        {
            if (dependency.Type == ViewModuleType.Script &&
                dependency.Name == "nxkit-xforms.js")
                return typeof(ViewModuleDependencyResolver).Assembly.GetManifestResourceStream("NXKit.Web.UI.Scripts.nxkit-xforms.js");
            if (dependency.Type == ViewModuleType.Css &&
                dependency.Name == "nxkit-xforms.css")
                return typeof(ViewModuleDependencyResolver).Assembly.GetManifestResourceStream("NXKit.Web.UI.Scripts.nxkit-xforms.css");
            if (dependency.Type == ViewModuleType.View &&
                dependency.Name == "nxkit-xforms.html")
                return typeof(ViewModuleDependencyResolver).Assembly.GetManifestResourceStream("NXKit.Web.UI.Scripts.nxkit-xforms.html");

            return null;
        }

    }

}
