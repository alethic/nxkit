using System;
using System.ComponentModel.Composition;
using System.Web;

namespace NXKit.View.Web.UI
{

    [Export(typeof(IViewModuleResolver))]
    public class ViewModuleResolver :
        IViewModuleResolver
    {

        static readonly Action<HttpResponse> nxkit_js = req =>
        {
            req.ContentType = "application/javascript";
            typeof(ViewModuleResolver).Assembly.GetManifestResourceStream("NXKit.View.Web.UI.Scripts.nxkit.js").CopyTo(req.OutputStream);
        };

        static readonly Action<HttpResponse> nxkit_css = req =>
        {
            req.ContentType = "text/css";
            typeof(ViewModuleResolver).Assembly.GetManifestResourceStream("NXKit.View.Web.UI.Content.nxkit.css").CopyTo(req.OutputStream);
        };

        static readonly Action<HttpResponse> nxkit_html = req =>
        {
            req.ContentType = "text/html";
            typeof(ViewModuleResolver).Assembly.GetManifestResourceStream("NXKit.View.Web.UI.Content.nxkit.html").CopyTo(req.OutputStream);
        };

        public Action<HttpResponse> Resolve(string name)
        {
            if (name == "nxkit")
                return nxkit_js;
            if (name == "nxkit.css")
                return nxkit_css;
            if (name == "nxkit.html")
                return nxkit_html;

            return null;
        }

    }

}
