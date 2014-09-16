using System.IO;
using System.Web.Hosting;
using System.Web.Http;
using Newtonsoft.Json.Linq;

namespace NXKit.Web.Ng.Test.Site.Controllers
{

    public class ViewController :
        ApiController
    {

        public object Get(string name)
        {
            var nx = new NXKit.Web.ViewServer();
            nx.Load(new FileInfo(Path.Combine(HostingEnvironment.MapPath("~"), @"../NXKit.XForms.Examples/" + name + ".xml")).FullName);
            return nx.Save();
        }

        public object Post(string name, JObject request)
        {
            var nx = new NXKit.Web.ViewServer();
            nx.Load(request);
            return nx.Save();
        }

    }

}
