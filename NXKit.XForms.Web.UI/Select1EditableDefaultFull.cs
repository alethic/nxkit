using System;
using System.Diagnostics.Contracts;

namespace NXKit.XForms.Web.UI
{

    [XFormsAppearance(Constants.XForms_1_0_NS, "full")]
    [Priority(-128)]
    public class Select1EditableDefaultFull :
        Select1EditableStringFull
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="visual"></param>
        public Select1EditableDefaultFull(NXKit.Web.UI.View view, XFormsSelect1Visual visual)
            : base(view, visual)
        {
            Contract.Requires<ArgumentNullException>(view != null);
            Contract.Requires<ArgumentNullException>(visual != null);
        }

    }

}
