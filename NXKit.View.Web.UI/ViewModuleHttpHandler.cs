using System;
using System.Linq;
using System.Net;
using System.Web;

using NXKit.Composition;
using NXKit.View.Js;

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
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;

            // look up module by name
            var name = context.Request.QueryString["m"];
            var file = CompositionUtil.CreateContainer(CompositionUtil.DefaultGlobalCatalog)
                .GetExportedValues<IViewModuleResolver>()
                .SelectMany(i => i.Resolve(name))
                .FirstOrDefault(i => i != null);

            if (file != null)
            {
                var ifNoneMatch = context.Request.Headers["If-None-Match"];
                if (ifNoneMatch != null)
                {
                    var p = ifNoneMatch.IndexOf(",", StringComparison.Ordinal);
                    if (p > -1)
                        ifNoneMatch = ifNoneMatch.Substring(0, p);

                    if (file.ETag == ifNoneMatch)
                    {
                        context.Response.ClearContent();
                        context.Response.StatusCode = (int)HttpStatusCode.NotModified;
                        context.Response.SuppressContent = true;
                        return;
                    }
                }

                var ifModifiedSince = context.Request.Headers["If-Modified-Since"];
                if (ifModifiedSince != null)
                {
                    DateTime d;
                    if (DateTime.TryParse(ifModifiedSince, out d) && file.LastModifiedTime <= d)
                    {
                        context.Response.ClearContent();
                        context.Response.StatusCode = (int)HttpStatusCode.NotModified;
                        context.Response.SuppressContent = true;
                        return;
                    }
                }

                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.ContentType = file.ContentType;
                context.Response.Cache.SetLastModified(file.LastModifiedTime);
                context.Response.Cache.SetETag(file.ETag);
                file.Writer(context.Response.OutputStream);
            }
        }

    }

}
