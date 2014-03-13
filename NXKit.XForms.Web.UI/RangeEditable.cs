using System;
using System.Diagnostics.Contracts;

using NXKit.Web.UI;

namespace NXKit.XForms.Web.UI
{

    /// <summary>
    /// Abstract base class for the editable portion of an XForms range control.
    /// </summary>
    public abstract class RangeEditable :
        SingleNodeBindingVisualControl<XFormsRangeVisual>,
        IRangeEditable
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        protected RangeEditable(View view, XFormsRangeVisual visual)
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
