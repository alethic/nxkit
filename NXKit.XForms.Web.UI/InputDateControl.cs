using System;
using System.Diagnostics.Contracts;
using System.Web.UI;

using Telerik.Web.UI;
using Telerik.Web.UI.Calendar;

using NXKit.Web.UI;

namespace NXKit.XForms.Web.UI
{

    public class InputDateControl :
        SingleNodeBindingVisualControl<XFormsInputVisual>
    {

        RadDatePicker ctl;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="visual"></param>
        public InputDateControl(View view, XFormsInputVisual visual)
            : base(view, visual)
        {
            Contract.Requires<ArgumentNullException>(view != null);
            Contract.Requires<ArgumentNullException>(visual != null);
        }

        protected override void OnVisualValueChanged()
        {
            base.OnVisualValueChanged();
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

            var labelVisual = Visual.FindLabelVisual();
            if (labelVisual != null)
                ctl.DateInput.Label = labelVisual.ToText();

            Controls.Add(ctl);
        }

        void ctl_SelectedDateChanged(object sender, SelectedDateChangedEventArgs args)
        {
            BindingUtil.Set(Visual.Binding, ctl.SelectedDate);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            // client-side control element
            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "XForms_Input XForms_Input_Date");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            base.Render(writer);
            writer.RenderEndTag();
        }

    }

}
