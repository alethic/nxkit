using System.Web.UI;

[assembly: WebResource("NXKit.Web.UI.Scripts.jquery-2.1.0.js", "text/javascript")]
[assembly: WebResource("NXKit.Web.UI.Scripts.knockout-3.2.0.js", "text/javascript")]
[assembly: WebResource("NXKit.Web.UI.Scripts.nxkit.js", "text/javascript")]
[assembly: WebResource("NXKit.Web.UI.Scripts.View.js", "text/javascript")]

[assembly: System.Web.PreApplicationStartMethod(typeof(NXKit.View.Web.UI.PreApplication), "Start")]

namespace NXKit.View.Web.UI
{

    public static class PreApplication
    {

        public static void Start()
        {
            if (ScriptManager.ScriptResourceMapping.GetDefinition("jquery") == null)
                ScriptManager.ScriptResourceMapping.AddDefinition("jquery", new ScriptResourceDefinition()
                {
                    ResourceAssembly = typeof(PreApplication).Assembly,
                    ResourceName = "NXKit.Web.UI.Scripts.jquery-2.1.1.js",
                    CdnPath = "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-2.1.1.min.js",
                    CdnDebugPath = "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-2.1.1.js",
                    CdnSupportsSecureConnection = true,
                    LoadSuccessExpression = "window.jQuery"
                });


            if (ScriptManager.ScriptResourceMapping.GetDefinition("knockout") == null)
                ScriptManager.ScriptResourceMapping.AddDefinition("knockout", new ScriptResourceDefinition()
                {
                    ResourceAssembly = typeof(PreApplication).Assembly,
                    ResourceName = "NXKit.Web.UI.Scripts.knockout-3.2.0.js",
                    CdnPath = "//cdnjs.cloudflare.com/ajax/libs/knockout/3.2.0/knockout-min.js",
                    CdnDebugPath = "//cdnjs.cloudflare.com/ajax/libs/knockout/3.2.0/knockout.js",
                    CdnSupportsSecureConnection = true,
                    LoadSuccessExpression = "window.ko",
                });

            if (ScriptManager.ScriptResourceMapping.GetDefinition("nxkit") == null)
                ScriptManager.ScriptResourceMapping.AddDefinition("nxkit", new ScriptResourceDefinition()
                {
                    ResourceAssembly = typeof(PreApplication).Assembly,
                    ResourceName = "NXKit.Web.UI.Scripts.nxkit.js",
                    LoadSuccessExpression = "window.NXKit",
                });

            if (ScriptManager.ScriptResourceMapping.GetDefinition("nxkit-ui") == null)
                ScriptManager.ScriptResourceMapping.AddDefinition("nxkit-ui", new ScriptResourceDefinition()
                {
                    ResourceAssembly = typeof(PreApplication).Assembly,
                    ResourceName = "NXKit.Web.UI.Scripts.View.js",
                    LoadSuccessExpression = "window._NXKit",
                });
        }

    }

}