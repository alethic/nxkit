using System;
using System.Diagnostics.Contracts;

using Telerik.Web.UI;
using Telerik.Web.UI.Calendar;

using NXKit.Web.UI;

namespace NXKit.XForms.Web.UI
{

    [XFormsXsdType(XmlSchemaConstants.XMLSchema_NS, "date")]
    public class InputEditableDate :
        InputEditable
    {

        RadDatePicker ctl;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        public InputEditableDate(View view, XFormsInputVisual visual)
            : base(view, visual)
        {
            Contract.Requires<ArgumentNullException>(view != null);
            Contract.Requires<ArgumentNullException>(visual != null);
        }

        protected override void OnVisualValueChanged()
        {
            ctl.SelectedDate = BindingUtil.Get<DateTime?>(Visual.Binding);
        }

        protected override void OnVisualReadOnlyOrReadWrite()
        {
            ctl.Enabled = !Visual.ReadOnly;
        }

        /// <summary>
        /// Creates the child control hierarchy.
        /// </summary>
        protected override void CreateChildControls()
        {
            ctl = new RadDatePicker();
            ctl.ID = "ctl";
            ctl.Calendar.EnableMonthYearFastNavigation = true;
            ctl.SelectedDateChanged += ctl_SelectedDateChanged;

            // TODO set to constraint, if any
            ctl.MinDate = DateTime.MinValue;
            ctl.MaxDate = DateTime.MaxValue;
            Controls.Add(ctl);
        }

        void ctl_SelectedDateChanged(object sender, SelectedDateChangedEventArgs args)
        {
            BindingUtil.Set(Visual.Binding, ctl.SelectedDate);
        }

    }

}
