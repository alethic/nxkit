using System;
using System.Linq;
using System.Web;

using NXKit.Composition;

namespace NXKit.View.Web.UI
{

    /// <summary>
    /// Handles NXKit.axd requests.
    /// </summary>
    public class ViewModuleHttpHandler :
        IHttpHandler
    {

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            // allow browser to cache response
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.Cache.SetMaxAge(new TimeSpan(1, 0, 0));
            context.Response.StatusCode = 404;

            // look up module by name
            var name = context.Request.QueryString["m"];
            var file = CompositionUtil.CreateContainer(CompositionUtil.DefaultGlobalCatalog)
                .GetExportedValues<IViewModuleResolver>()
                .Select(i => i.Resolve(name))
                .FirstOrDefault(i => i != null);

            if (file != null)
            {
                // send module data
                context.Response.StatusCode = 200;
                file(context.Response);
            }
        }

    }

}
