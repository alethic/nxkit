using System;
using System.Web.UI;

namespace NXKit.Test.Web.Site
{

    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            ScriptManager.ScriptResourceMapping.AddDefinition("less", new ScriptResourceDefinition
            {
                Path = "~/Scripts/less-1.7.0.min.js",
                DebugPath = "~/Scripts/less-1.7.0.js",
                CdnPath = "//cdnjs.cloudflare.com/ajax/libs/less.js/1.7.0/less.min.js",
                CdnDebugPath = "//cdnjs.cloudflare.com/ajax/libs/less.js/1.7.0/less.js"
            });

            ScriptManager.ScriptResourceMapping.AddDefinition("semantic", new ScriptResourceDefinition
            {
                Path = "~/Content/semantic/packaged/javascript/semantic.js",
            });

            ScriptManager.ScriptResourceMapping.AddDefinition("knockout", new ScriptResourceDefinition
            {
                Path = "~/Scripts/knockout-3.1.0.min.js",
                DebugPath = "~/Scripts/knockout-3.1.0.js",
            });
        }

    }

}