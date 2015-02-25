using System.Web.UI;

[assembly: WebResource("NXKit.View.Web.UI.Scripts.jquery-2.1.3.js", "text/javascript")]
[assembly: WebResource("NXKit.View.Web.UI.Scripts.knockout-3.2.0.js", "text/javascript")]
[assembly: WebResource("NXKit.View.Web.UI.Scripts.nx-require.js", "text/javascript")]
[assembly: WebResource("NXKit.View.Web.UI.Scripts.nxkit-ui.js", "text/javascript")]

[assembly: System.Web.PreApplicationStartMethod(typeof(NXKit.View.Web.UI.PreApplication), "Start")]

namespace NXKit.View.Web.UI
{

    public static class PreApplication
    {

        public static void Start()
        {
            if (ScriptManager.ScriptResourceMapping.GetDefinition("nx-require") == null)
                ScriptManager.ScriptResourceMapping.AddDefinition("nx-require", new ScriptResourceDefinition()
                {
                    ResourceAssembly = typeof(PreApplication).Assembly,
                    ResourceName = "NXKit.View.Web.UI.Scripts.nx-require.js",
                    CdnSupportsSecureConnection = true,
                    LoadSuccessExpression = "NXKit.require"
                });

            if (ScriptManager.ScriptResourceMapping.GetDefinition("jquery") == null)
                ScriptManager.ScriptResourceMapping.AddDefinition("jquery", new ScriptResourceDefinition()
                {
                    ResourceAssembly = typeof(PreApplication).Assembly,
                    ResourceName = "NXKit.View.Web.UI.Scripts.jquery-2.1.3.js",
                    CdnPath = "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-2.1.3.min.js",
                    CdnDebugPath = "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-2.1.3.js",
                    CdnSupportsSecureConnection = true,
                    LoadSuccessExpression = "window.jQuery"
                });

            if (ScriptManager.ScriptResourceMapping.GetDefinition("knockout") == null)
                ScriptManager.ScriptResourceMapping.AddDefinition("knockout", new ScriptResourceDefinition()
                {
                    ResourceAssembly = typeof(PreApplication).Assembly,
                    ResourceName = "NXKit.View.Web.UI.Scripts.knockout-3.2.0.js",
                    CdnPath = "//cdnjs.cloudflare.com/ajax/libs/knockout/3.2.0/knockout-min.js",
                    CdnDebugPath = "//cdnjs.cloudflare.com/ajax/libs/knockout/3.2.0/knockout.js",
                    CdnSupportsSecureConnection = true,
                    LoadSuccessExpression = "window.ko",
                });

            if (ScriptManager.ScriptResourceMapping.GetDefinition("nxkit-ui") == null)
                ScriptManager.ScriptResourceMapping.AddDefinition("nxkit-ui", new ScriptResourceDefinition()
                {
                    ResourceAssembly = typeof(PreApplication).Assembly,
                    ResourceName = "NXKit.View.Web.UI.Scripts.nxkit-ui.js",
                    LoadSuccessExpression = "window._NXKit",
                });
        }

    }

}