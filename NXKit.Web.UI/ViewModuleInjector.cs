using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Web.UI;

namespace NXKit.Web.UI
{

    [Export(typeof(IViewInjector))]
    public class ViewModuleInjector :
        IViewInjector
    {

        public void OnPreRender(View view)
        {
            if (view.EnableScriptManager)
            {
                view.Page.ClientScript.RegisterStartupScript(typeof(View), "nxkit-define", @"_NXKit.Web.UI.defines['nxkit.js'] = window['NXKit'];", true);
            }
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
