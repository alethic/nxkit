using System;
using System.Diagnostics.Contracts;

using Telerik.Web.UI;
using Telerik.Web.UI.Calendar;

using NXKit.Web.UI;

namespace NXKit.XForms.Web.UI
{

    [XFormsXsdType(XmlSchemaConstants.XMLSchema_NS, "time")]
    public class InputEditableTime : 
        InputEditable
    {

        RadTimePicker ctl;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        public InputEditableTime(View view, XFormsInputVisual visual)
            : base(view, visual)
        {
            Contract.Requires<ArgumentNullException>(view != null);
            Contract.Requires<ArgumentNullException>(visual != null);
        }

        /// <summary>
        /// Creates the child control hierarchy.
        /// </summary>
        protected override void CreateChildControls()
        {
            ctl = new RadTimePicker();
            ctl.ID = "ctl";
            ctl.SelectedDate = BindingUtil.Get<DateTime?>(Visual.Binding);
            ctl.SelectedDateChanged += ctl_SelectedDateChanged;
            Controls.Add(ctl);
        }

        void ctl_SelectedDateChanged(object sender, SelectedDateChangedEventArgs args)
        {
            BindingUtil.Set(Visual.Binding, ctl.SelectedDate);
        }

    }

}
