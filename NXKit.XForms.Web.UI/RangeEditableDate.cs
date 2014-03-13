using System;
using System.Diagnostics.Contracts;

using Telerik.Web.UI;
using Telerik.Web.UI.Calendar;

using NXKit.Web.UI;

namespace NXKit.XForms.Web.UI
{

    [XFormsXsdType(XmlSchemaConstants.XMLSchema_NS, "date")]
    public class RangeEditableDate :
        RangeEditable
    {

        RadDatePicker ctl;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        public RangeEditableDate(View view, XFormsRangeVisual visual)
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
            ctl = new RadDatePicker();
            ctl.ID = "ctl";
            ctl.EnableViewState = false;
            ctl.Calendar.EnableMonthYearFastNavigation = true;
            ctl.SelectedDateChanged += ctl_SelectedDateChanged;

            ctl.MinDate = DateTime.MinValue;
            ctl.MaxDate = DateTime.MaxValue;

            DateTime startDate;
            if (DateTime.TryParse(Visual.Start, out startDate))
                ctl.MinDate = startDate;

            DateTime endDate;
            if (DateTime.TryParse(Visual.End, out endDate))
                ctl.MaxDate = endDate;

            Controls.Add(ctl);
        }

        void ctl_SelectedDateChanged(object sender, SelectedDateChangedEventArgs args)
        {
            BindingUtil.Set(Visual.Binding, ctl.SelectedDate);
        }

        public override string TargetID
        {
            get { return ctl.ClientID; }
        }

        protected override void OnVisualValueChanged()
        {
            ctl.SelectedDate = BindingUtil.Get<DateTime?>(Visual.Binding);
        }

        protected override void OnVisualReadOnlyOrReadWrite()
        {
            ctl.Enabled = !Visual.ReadOnly;
        }

    }

}
