using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Web.UI;

using NXKit.Web.UI;

namespace NXKit.XForms.View.Web.UI
{

    [Export(typeof(IViewInjector))]
    public class ViewModuleInjector :
        IViewInjector
    {

        public void OnPreRender(NXKit.Web.UI.View view)
        {

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
