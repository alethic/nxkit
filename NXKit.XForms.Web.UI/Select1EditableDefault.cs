using System;
using System.Diagnostics.Contracts;

namespace NXKit.XForms.Web.UI
{

    [Priority(-256)]
    public class Select1EditableDefault :
        Select1EditableString
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        public Select1EditableDefault(NXKit.Web.UI.View view, XFormsSelect1Visual visual)
            : base(view, visual)
        {
            Contract.Requires<ArgumentNullException>(view != null);
            Contract.Requires<ArgumentNullException>(visual != null);
        }

    }

}
