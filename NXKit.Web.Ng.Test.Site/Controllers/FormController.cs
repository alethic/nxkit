using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Hosting;
using System.Web.Http;

namespace NXKit.Web.Ng.Test.Site.Controllers
{

    public class FormController :
        ApiController
    {

        public object Get(string name)
        {
            var dat = File.ReadAllBytes(Path.Combine(HostingEnvironment.MapPath("~"), @"../NXKit.XForms.Examples/" + name + ".xml"));

            var msg = new HttpResponseMessage(HttpStatusCode.OK);
            msg.Content = new ByteArrayContent(dat);
            msg.Content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
            return msg;
        }

    }

}
