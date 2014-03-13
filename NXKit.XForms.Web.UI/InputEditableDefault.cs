using System;
using System.Diagnostics.Contracts;

namespace NXKit.XForms.Web.UI
{

    [Priority(int.MinValue)]
    public class InputEditableDefault :
        InputEditableString
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        public InputEditableDefault(NXKit.Web.UI.View view, XFormsInputVisual visual)
            : base(view, visual)
        {
            Contract.Requires<ArgumentNullException>(view != null);
            Contract.Requires<ArgumentNullException>(visual != null);
        }

    }

}