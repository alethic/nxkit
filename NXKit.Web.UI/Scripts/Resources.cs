using System.Web.UI;

[assembly: WebResource("NXKit.Web.UI.Scripts.jquery-2.1.1.js", "text/javascript")]
[assembly: WebResource("NXKit.Web.UI.Scripts.jquery-2.1.1.min.js", "text/javascript")]
[assembly: WebResource("NXKit.Web.UI.Scripts.knockout-3.2.0.debug.js", "text/javascript")]
[assembly: WebResource("NXKit.Web.UI.Scripts.knockout-3.2.0.js", "text/javascript")]
[assembly: WebResource("NXKit.Web.UI.Scripts.View.js", "text/javascript")]

[assembly: System.Web.PreApplicationStartMethod(typeof(Register), "Start")]
public static class Register
{

    public static void Start()
    {
        if (ScriptManager.ScriptResourceMapping.GetDefinition("jquery") == null)
            ScriptManager.ScriptResourceMapping.AddDefinition("jquery", new ScriptResourceDefinition()
            {
                ResourceAssembly = typeof(Register).Assembly,
                ResourceName = "NXKit.Web.UI.Scripts.jquery-2.1.1.min.js",
            });

        if (ScriptManager.ScriptResourceMapping.GetDefinition("knockout") == null)
            ScriptManager.ScriptResourceMapping.AddDefinition("knockout", new ScriptResourceDefinition()
            {
                
                ResourceAssembly = typeof(Register).Assembly,
                ResourceName = "NXKit.Web.UI.Scripts.knockout-3.2.0.js",
            });

        if (ScriptManager.ScriptResourceMapping.GetDefinition("nxkit-ui") == null)
            ScriptManager.ScriptResourceMapping.AddDefinition("nxkit-ui", new ScriptResourceDefinition()
            {
                ResourceAssembly = typeof(Register).Assembly,
                ResourceName = "NXKit.Web.UI.Scripts.View.js",
            });
    }

}