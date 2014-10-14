using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

using NXKit.View.Js;

namespace NXKit.View.Server.Ng.Test.Site.Controllers
{

    [RoutePrefix("api/view")]
    public class ViewController :
        ApiController
    {

        static readonly ViewServer server = new ViewServer();

        [Route("module/{name}.js")]
        [Route("module/{name}")]
        [HttpGet]
        public object GetModule(string name)
        {
            var module = ViewModuleProvider.ResolveViewModule(name);
            if (module == null)
                return NotFound();

            var eTagHeaderValue = new EntityTagHeaderValue("\"" + module.ETag + "\"");
            if (Request.Headers.IfNoneMatch.Contains(eTagHeaderValue))
                return new HttpResponseMessage(HttpStatusCode.NotModified);

            var lastModifiedTime = (DateTimeOffset?)module.LastModifiedTime;
            if (Request.Headers.IfModifiedSince >= lastModifiedTime)
                return new HttpResponseMessage(HttpStatusCode.NotModified);

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Headers.ETag = eTagHeaderValue;
            response.Headers.CacheControl = new CacheControlHeaderValue() { Public = true };
            response.Content = new PushStreamContent((s, c, t) => { module.Write(s); s.Close(); });
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(module.ContentType);
            response.Content.Headers.LastModified = lastModifiedTime;
            return response;
        }

        [Route("{name}")]
        [HttpGet]
        public Task<ViewMessage> Get(string name)
        {
            return Task.FromResult(server.Load(new Uri(Request.RequestUri, "/Examples/" + name + ".xml")));
        }

        [Route("{name}")]
        [HttpPost]
        public Task<ViewMessage> Post(string name, ViewMessage message)
        {
            return Task.FromResult(server.Load(message));
        }

    }

}
