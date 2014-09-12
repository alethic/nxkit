using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

using NXKit.XForms.Examples;

namespace NXKit.Test.Web.Site
{

    public class ExampleHttpHandler :
        IHttpHandler
    {

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            var path = context.Request.Path;
            if (path.StartsWith("/Examples/"))
                ProcessRequest(context, path.Remove(0, 10));
        }

        void ProcessRequest(HttpContext context, string path)
        {
            Contract.Requires<ArgumentNullException>(context != null);

            if (context.Request.HttpMethod == "PUT" ||
                context.Request.HttpMethod == "POST")
                return;

            var stm = typeof(Ref).Assembly
                .GetManifestResourceNames()
                .Where(i => i.EndsWith("." + path))
                .Select(i => typeof(Ref).Assembly.GetManifestResourceStream(i))
                .FirstOrDefault();
            if (stm == null)
            {
                context.Response.StatusCode = 404;
                return;
            }

            stm.CopyTo(context.Response.OutputStream);

            if (path.EndsWith(".xml"))
                context.Response.ContentType = "application/xml";
        }

    }

}