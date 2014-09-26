using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using NXKit.Web.UI;

namespace NXKit.XForms.View.Web.UI
{

    [Export(typeof(IViewInjector))]
    public class ViewModuleInjector :
        IViewInjector
    {

        public void OnPreRender(NXKit.Web.UI.View view)
        {
            var sb = new StringBuilder();

            if (view.EnableScriptManager)
            {
                sb.AppendLine(@"_NXKit.Web.UI.defines['nxkit-xforms'] = window['NXKit'];");
            }

            if (view.EnableEmbeddedStyles)
            {
                var link = new HtmlLink();
                link.Href = view.Page.ClientScript.GetWebResourceUrl(typeof(ViewModuleInjector), "NXKit.XForms.View.Web.UI.Scripts.nxkit-xforms.css");
                link.Attributes["rel"] = "stylesheet";
                link.Attributes["type"] = "text/css";
                link.Attributes["data-nx-require"] = "css!nxkit-xforms.css";
                view.Page.Header.Controls.Add(link);

                sb.AppendLine(@"_NXKit.Web.UI.defines['nxkit-xforms.css'] = $('" + link.ClientID + @"')[0];");
            }

            var cd = sb.ToString();
            if (cd != "")
                view.Page.ClientScript.RegisterStartupScript(typeof(ViewModuleInjector), "nxkit-xforms-define", cd, true);
        }

        public void OnRender(NXKit.Web.UI.View view, HtmlTextWriter writer)
        {

        }

        public IEnumerable<ScriptReference> GetScriptReferences(NXKit.Web.UI.View view)
        {
            yield return new ScriptReference()
            {
                Name = "nxkit-xforms",
            };
        }

    }

}
