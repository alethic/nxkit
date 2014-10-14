using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace NXKit.View.Server.Ng.Test.Site.Controllers
{

    public class ViewController :
        ApiController
    {

        static readonly ViewServer server = new ViewServer();

        public Task<ViewMessage> Get(string name)
        {
            return Task.FromResult(server.Load(new Uri(Request.RequestUri, "/Examples/" + name + ".xml")));
        }

        public Task<ViewMessage> Post(string name, ViewMessage message)
        {
            return Task.FromResult(server.Load(message));
        }

    }

}
