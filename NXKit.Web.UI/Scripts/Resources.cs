using System.Web.UI;

[assembly: WebResource("NXKit.Web.UI.Scripts.View.js", "text/javascript")]

[assembly: System.Web.PreApplicationStartMethod(typeof(Register), "Start")]
public static class Register
{

    public static void Start()
    {
        if (ScriptManager.ScriptResourceMapping.GetDefinition("nxkit-ui") == null)
            ScriptManager.ScriptResourceMapping.AddDefinition("nxkit-ui", new ScriptResourceDefinition()
            {
                ResourceAssembly = typeof(Register).Assembly,
                ResourceName = "NXKit.Web.UI.Scripts.View.js",
            });
    }

}