using System.Web.UI;

[assembly: WebResource("NXKit.XForms.Layout.View.Web.UI.Scripts.nxkit-xforms-layout.js", "application/javascript")]

[assembly: System.Web.PreApplicationStartMethod(typeof(NXKit.XForms.Layout.View.Web.UI.PreApplication), "Start")]

namespace NXKit.XForms.Layout.View.Web.UI
{

    public static class PreApplication
    {

        public static void Start()
        {
            if (ScriptManager.ScriptResourceMapping.GetDefinition("nxkit-xforms-layout") == null)
                ScriptManager.ScriptResourceMapping.AddDefinition("nxkit-xforms-layout", new ScriptResourceDefinition()
                {
                    ResourceAssembly = typeof(PreApplication).Assembly,
                    ResourceName = "NXKit.XForms.Layout.View.Web.UI.Scripts.nxkit-xforms-layout.js",
                    LoadSuccessExpression = "window.NXKit.XForms.Layout",
                });
        }

    }

}