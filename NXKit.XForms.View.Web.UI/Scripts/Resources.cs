using System.Web.UI;

[assembly: WebResource("NXKit.XForms.View.Web.UI.Scripts.nxkit-xforms.js", "application/javascript")]

[assembly: System.Web.PreApplicationStartMethod(typeof(NXKit.XForms.View.Web.UI.PreApplication), "Start")]

namespace NXKit.XForms.View.Web.UI
{

    public static class PreApplication
    {

        public static void Start()
        {
            if (ScriptManager.ScriptResourceMapping.GetDefinition("nxkit-xforms") == null)
                ScriptManager.ScriptResourceMapping.AddDefinition("nxkit-xforms", new ScriptResourceDefinition()
                {
                    ResourceAssembly = typeof(PreApplication).Assembly,
                    ResourceName = "NXKit.XForms.View.Web.UI.Scripts.nxkit-xforms.js",
                    LoadSuccessExpression = "window.NXKit.XForms",
                });
        }

    }

}