using System;
using System.ComponentModel.Composition;
using System.Web;

using NXKit.IO.Media;

namespace NXKit.View.Web.UI
{

    [Export(typeof(IViewModuleResolver))]
    public class ViewModuleResolver :
        IViewModuleResolver
    {

        static Action<HttpResponse> GetResourceResponse(string relativePath, MediaType mediaType)
        {
            var name = "NXKit.View.Web.UI." + relativePath;
            var file = typeof(ViewModuleResolver).Assembly.GetManifestResourceStream(name);
            if (file != null)
            {
                return req =>
                {
                    req.ContentType = mediaType;
                    file.CopyTo(req.OutputStream);
                };
            }

            return null;
        }

        public Action<HttpResponse> Resolve(string name)
        {
            if (name == "nxkit")
                return GetResourceResponse("Scripts.nxkit.js", "application/javascript");
            if (name == "nxkit.css")
                return GetResourceResponse("Content.nxkit.css", "text/css");
            if (name == "nxkit.html")
                return GetResourceResponse("Content.nxkit.html", "text/html");
            if (name == "nx-html")
                return GetResourceResponse("Scripts.nx-html.js", "application/javascript");
            if (name == "nx-css")
                return GetResourceResponse("Scripts.nx-css.js", "application/javascript");

            return null;
        }

    }

}
