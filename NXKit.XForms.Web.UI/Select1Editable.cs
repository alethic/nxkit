using System;
using System.Diagnostics.Contracts;

using NXKit.Web.UI;

namespace NXKit.XForms.Web.UI
{

    /// <summary>
    /// Abstract base class for the editable portion of an XForms input control.
    /// </summary>
    public abstract class Select1Editable :
        SingleNodeBindingVisualControl<XFormsSelect1Visual>,
        ISelect1Editable
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        public Select1Editable(View view, XFormsSelect1Visual visual)
            : base(view, visual)
        {
            Contract.Requires<ArgumentNullException>(view != null);
            Contract.Requires<ArgumentNullException>(visual != null);
        }

        /// <summary>
        /// Gets the ID to be used to target the editable.
        /// </summary>
        public virtual string TargetID
        {
            get { return ClientID; }
        }

    }

}
