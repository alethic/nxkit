using System;
using System.Web.Http;

using Newtonsoft.Json.Linq;

namespace NXKit.View.Server.Ng.Test.Site.Controllers
{

    public class ViewController :
        ApiController
    {

        static readonly ViewServer server = new ViewServer();

        public object Get(string name)
        {
            return server.Load(new Uri(Request.RequestUri, "/Examples/" + name + ".xml"));
        }

        public object Post(string name, ViewMessage message)
        {
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All;
            return server.Load(message);
        }

    }

}
