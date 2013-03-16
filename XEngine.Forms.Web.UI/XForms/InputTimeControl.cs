using System;

using Telerik.Web.UI;
using Telerik.Web.UI.Calendar;

using XEngine.Forms;

namespace XEngine.Forms.Web.UI.XForms
{

    public class InputTimeControl : VisualControl<XFormsInputVisual>
    {

        private RadTimePicker ctl;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="visual"></param>
        public InputTimeControl(FormView view, XFormsInputVisual visual)
            : base(view, visual)
        {

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

        private void ctl_SelectedDateChanged(object sender, SelectedDateChangedEventArgs args)
        {
            BindingUtil.Set(Visual.Binding, ctl.SelectedDate);
        }

    }

}
