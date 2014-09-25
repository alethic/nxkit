using System.Collections.Generic;
using System.Web.UI;

namespace NXKit.Web.UI
{

    /// <summary>
    /// Provides points to inject information into the output control.
    /// </summary>
    public interface IViewInjector
    {

        /// <summary>
        /// Invoked during the PreRender phase.
        /// </summary>
        /// <param name="view"></param>
        void OnPreRender(View view);

        /// <summary>
        /// Invoked during the render phase.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="writer"></param>
        void OnRender(View view, HtmlTextWriter writer);

        /// <summary>
        /// Provides <see cref="ScriptReference"/> instances for the <see cref="View"/>.
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        IEnumerable<ScriptReference> GetScriptReferences(View view);

    }

}
