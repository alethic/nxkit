using System;
using System.ComponentModel.Composition;
using System.Web;
using NXKit.Web.UI;

namespace NXKit.XForms.View.Web.UI
{

    [Export(typeof(IViewModuleResolver))]
    public class ViewModuleResolver :
        IViewModuleResolver
    {

        static readonly Action<HttpResponse> nxkit_xforms_js = req =>
        {
            req.ContentType = "application/javascript";
            typeof(ViewModuleResolver).Assembly.GetManifestResourceStream("NXKit.XForms.View.Web.UI.Scripts.nxkit-xforms.js").CopyTo(req.OutputStream);
        };

        static readonly Action<HttpResponse> nxkit_xforms_css = req =>
        {
            req.ContentType = "text/css";
            typeof(ViewModuleResolver).Assembly.GetManifestResourceStream("NXKit.XForms.View.Web.UI.Content.nxkit-xforms.css").CopyTo(req.OutputStream);
        };

        static readonly Action<HttpResponse> nxkit_xforms_html = req =>
        {
            req.ContentType = "text/html";
            typeof(ViewModuleResolver).Assembly.GetManifestResourceStream("NXKit.XForms.View.Web.UI.Content.nxkit-xforms.html").CopyTo(req.OutputStream);
        };

        public Action<HttpResponse> Resolve(string name)
        {
            if (name == "nxkit-xforms")
                return nxkit_xforms_js;
            if (name == "nxkit-xforms.css")
                return nxkit_xforms_css;
            if (name == "nxkit-xforms.html")
                return nxkit_xforms_html;

            return null;
        }

    }

}
