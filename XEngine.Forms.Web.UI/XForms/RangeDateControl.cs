using System;
using System.Linq;
using System.Web.UI;

using Telerik.Web.UI;
using Telerik.Web.UI.Calendar;

using XEngine.Forms;

namespace XEngine.Forms.Web.UI.XForms
{

    public class RangeDateControl : VisualControl<XFormsRangeVisual>
    {

        private RadDatePicker ctl;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="visual"></param>
        public RangeDateControl(FormView view, XFormsRangeVisual visual)
            : base(view,visual)
        {

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

            var labelVisual = Visual.Children.OfType<XFormsLabelVisual>().FirstOrDefault();
            if (labelVisual != null)
                ctl.DateInput.EmptyMessage = FormHelper.LabelToString(labelVisual);

            Controls.Add(ctl);
        }

        private void ctl_SelectedDateChanged(object sender, SelectedDateChangedEventArgs args)
        {
            BindingUtil.Set(Visual.Binding, ctl.SelectedDate);
        }

        protected override void OnPreRender(EventArgs args)
        {
            base.OnPreRender(args);

            ctl.SelectedDate = BindingUtil.Get<DateTime?>(Visual.Binding);
            ctl.Enabled = !Visual.ReadOnly;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            // client-side control element
            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "XForms_Range XForms_Range_Date");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            base.Render(writer);
            writer.RenderEndTag();
        }

    }

}
