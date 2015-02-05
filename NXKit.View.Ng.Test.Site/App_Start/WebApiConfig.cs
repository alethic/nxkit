using System.Web.Http;

namespace NXKit.View.Server.Ng.Test.Site
{

    public static class WebApiConfig
    {

        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
        }

    }

}
