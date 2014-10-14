using System.Web.Http;

namespace NXKit.View.Server.Ng.Test.Site
{

    public static class WebApiConfig
    {

        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{name}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

    }

}
