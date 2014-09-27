using System;
using System.ComponentModel.Composition;
using System.Web;

using NXKit.Web.UI;

namespace NXKit.Web.XForms.Layout.View.UI
{

    [Export(typeof(IViewModuleResolver))]
    public class ViewModuleResolver :
        IViewModuleResolver
    {

        static readonly Action<HttpResponse> nxkit_xforms_layout_js = req =>
        {
            req.ContentType = "application/javascript";
            typeof(ViewModuleResolver).Assembly.GetManifestResourceStream("NXKit.XForms.Layout.View.Web.UI.Scripts.nxkit-xforms-layout.js").CopyTo(req.OutputStream);
        };

        static readonly Action<HttpResponse> nxkit_xforms_layout_css = req =>
        {
            req.ContentType = "text/css";
            typeof(ViewModuleResolver).Assembly.GetManifestResourceStream("NXKit.XForms.Layout.View.Web.UI.Content.nxkit-xforms-layout.css").CopyTo(req.OutputStream);
        };

        static readonly Action<HttpResponse> nxkit_xforms_layout_html = req =>
        {
            req.ContentType = "text/html";
            typeof(ViewModuleResolver).Assembly.GetManifestResourceStream("NXKit.XForms.Layout.View.Web.UI.Content.nxkit-xforms-layout.html").CopyTo(req.OutputStream);
        };

        public Action<HttpResponse> Resolve(string name)
        {
            if (name == "nxkit-xforms-layout")
                return nxkit_xforms_layout_js;
            if (name == "nxkit-xforms-layout.css")
                return nxkit_xforms_layout_css;
            if (name == "nxkit-xforms-layout.html")
                return nxkit_xforms_layout_html;

            return null;
        }

    }

}
