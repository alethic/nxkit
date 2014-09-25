using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Web.UI;

namespace NXKit.View.UI.Test.Site
{

    public class Global : 
        System.Web.HttpApplication
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

            ScriptManager.ScriptResourceMapping.AddDefinition("bootstrap", new ScriptResourceDefinition
            {
                Path = "~/Content/bootstrap/dist/js/bootstrap.min.js",
                DebugPath = "~/Content/bootstrap/dist/js/bootstrap.js",
            });
        }

    }

}