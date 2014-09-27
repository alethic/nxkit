using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using NXKit.Web.UI;

namespace NXKit.XForms.Layout.View.Web.UI
{

    [Export(typeof(IViewInjector))]
    public class ViewModuleInjector :
        IViewInjector
    {

        public void OnPreRender(NXKit.Web.UI.View view)
        {
            var sb = new StringBuilder();

            if (view.EnableScriptManager &&
                view.EnableModuleScriptManagerScripts)
            {
                sb.AppendLine(@"(_NXKit.Web.UI.defines['nxkit-xforms-layout'] = $.Deferred()).resolve(window['NXKit']);");
            }

            if (view.EnableEmbeddedStyles)
            {
                var link = new HtmlLink();
                link.Href = view.Page.ClientScript.GetWebResourceUrl(typeof(ViewModuleInjector), "NXKit.XForms.Layout.View.Web.UI.Contents.nxkit-xforms-layout.css");
                link.Attributes["rel"] = "stylesheet";
                link.Attributes["type"] = "text/css";
                link.Attributes["data-nx-require"] = "css!nxkit-xforms-layout.css";
                view.Page.Header.Controls.Add(link);

                sb.AppendLine(@"(_NXKit.Web.UI.defines['nxkit-xforms-layout.css'] = $.Deferred()).resolve($('" + link.ClientID + @"')[0]);");
            }

            if (!string.IsNullOrWhiteSpace(sb.ToString()))
                view.Page.ClientScript.RegisterStartupScript(typeof(ViewModuleInjector), typeof(ViewModuleInjector).FullName, sb.ToString(), true);
        }

        public void OnRender(NXKit.Web.UI.View view, HtmlTextWriter writer)
        {

        }

        public IEnumerable<ScriptReference> GetScriptReferences(NXKit.Web.UI.View view)
        {
            yield return new ScriptReference()
            {
                Name = "nxkit-xforms-layout",
            };
        }

    }

}
