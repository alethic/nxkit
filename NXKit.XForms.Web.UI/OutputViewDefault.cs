using System;
using System.Diagnostics.Contracts;
using System.Web.UI;

using NXKit.Web.UI;

namespace NXKit.XForms.Web.UI
{

    /// <summary>
    /// Default output rendering.
    /// </summary>
    [Priority(int.MinValue)]
    public class OutputViewDefault :
        OutputView
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        public OutputViewDefault(View view, XFormsOutputVisual visual)
            : base(view, visual)
        {
            Contract.Requires<ArgumentNullException>(view != null);
            Contract.Requires<ArgumentNullException>(visual != null);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            
        }

    }

}
