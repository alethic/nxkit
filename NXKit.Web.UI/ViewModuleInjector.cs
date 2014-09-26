using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace NXKit.Web.UI
{

    [Export(typeof(IViewInjector))]
    public class ViewModuleInjector :
        IViewInjector
    {

        public void OnPreRender(View view)
        {
            var sb = new StringBuilder();

            if (view.EnableScriptManager)
            {
                sb.AppendLine(@"_NXKit.Web.UI.defines['nxkit'] = window['NXKit'];");
            }

            if (view.EnableEmbeddedStyles)
            {
                var link = new HtmlLink();
                link.Href = view.Page.ClientScript.GetWebResourceUrl(typeof(ViewModuleInjector), "NXKit.Web.UI.Scripts.nxkit.css");
                link.Attributes["rel"] = "stylesheet";
                link.Attributes["type"] = "text/css";
                link.Attributes["data-nx-require"] = "css!nxkit.css";
                view.Page.Header.Controls.Add(link);

                sb.AppendLine(@"_NXKit.Web.UI.defines['nxkit.css'] = $('"+ link.ClientID + @"')[0];");
            }

            var cd = sb.ToString();
            if (cd != "")
                view.Page.ClientScript.RegisterStartupScript(typeof(ViewModuleInjector), "nxkit-define", cd, true);
        }

        public void OnRender(View view, HtmlTextWriter writer)
        {

        }

        public IEnumerable<ScriptReference> GetScriptReferences(View view)
        {
            yield return new ScriptReference()
            {
                Name = "nxkit",
            };
        }

    }

}
