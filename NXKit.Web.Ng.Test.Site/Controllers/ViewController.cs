using System;
using System.Web.Http;

using Newtonsoft.Json.Linq;

namespace NXKit.Web.Ng.Test.Site.Controllers
{

    public class ViewController :
        ApiController
    {

        public object Get(string name)
        {
            using (var nx = new NXKit.Web.ViewServer())
            {
                nx.Load(new Uri(Request.RequestUri, "/Examples/" + name + ".xml"));
                return nx.Save();
            }
        }

        public object Post(string name, JObject request)
        {
            using (var nx = new NXKit.Web.ViewServer())
            {
                nx.Load(request);
                return nx.Save();
            }
        }

    }

}
