using System;
using System.Web.Http;

using Newtonsoft.Json.Linq;

namespace NXKit.Web.Ng.Test.Site.Controllers
{

    public class ViewController :
        ApiController
    {

        static readonly ViewServer server = new ViewServer();

        public object Get(string name)
        {
            return server.Load(new Uri(Request.RequestUri, "/Examples/" + name + ".xml"));
        }

        public object Post(string name, JObject args)
        {
            return server.Push(args);
        }

    }

}
