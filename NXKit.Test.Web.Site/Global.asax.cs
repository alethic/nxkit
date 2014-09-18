using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
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

            ScriptManager.ScriptResourceMapping.AddDefinition("bootstrap", new ScriptResourceDefinition
            {
                Path = "~/Content/bootstrap/dist/js/bootstrap.min.js",
                DebugPath = "~/Content/bootstrap/dist/js/bootstrap.js",
            });

            ScriptManager.ScriptResourceMapping.AddDefinition("knockout", new ScriptResourceDefinition
            {
                Path = "~/Scripts/knockout-3.1.0.js",
                DebugPath = "~/Scripts/knockout-3.1.0.debug.js",
            });

            ScriptManager.ScriptResourceMapping.AddDefinition("nxkit", new ScriptResourceDefinition
            {
                Path = "~/Scripts/nxkit.min.js",
                DebugPath = "~/Scripts/nxkit.js",
            });

            ScriptManager.ScriptResourceMapping.AddDefinition("nxkit-xforms", new ScriptResourceDefinition
            {
                Path = "~/Scripts/nxkit-xforms.min.js",
                DebugPath = "~/Scripts/nxkit-xforms.js",
            });

            ScriptManager.ScriptResourceMapping.AddDefinition("nxkit-xforms-layout", new ScriptResourceDefinition
            {
                Path = "~/Scripts/nxkit-xforms-layout.min.js",
                DebugPath = "~/Scripts/nxkit-xforms-layout.js",
            });
        }

    }

}