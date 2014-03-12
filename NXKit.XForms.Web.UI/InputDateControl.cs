using System;
using System.Linq;
using System.Web.UI;

using Telerik.Web.UI;
using Telerik.Web.UI.Calendar;

using NXKit.XForms;
using NXKit.Web.UI;

namespace NXKit.XForms.Web.UI
{

    public class InputDateControl : VisualControl<XFormsInputVisual>
    {

        RadDatePicker ctl;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="visual"></param>
        public InputDateControl(View view, XFormsInputVisual visual)
            : base(view, visual)
        {
            Visual.AddEventHandler<XFormsValueChangedEvent>(Visual_ValueChanged, false);
            Visual.AddEventHandler<XFormsReadOnlyEvent>(Visual_ReadOnlyChanged, false);
            Visual.AddEventHandler<XFormsReadWriteEvent>(Visual_ReadOnlyChanged, false);
        }

        /// <summary>
        /// Invoked when the value of the Visual changes.
        /// </summary>
        /// <param name="ev"></param>
        private void Visual_ValueChanged(Event ev)
        {
            EnsureChildControls();

            ctl.SelectedDate = BindingUtil.Get<DateTime?>(Visual.Binding);
        }

        /// <summary>
        /// Invoked when the read-only state of the Visual changes.
        /// </summary>
        /// <param name="ev"></param>
        private void Visual_ReadOnlyChanged(Event ev)
        {
            EnsureChildControls();

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
                ctl.DateInput.EmptyMessage = labelVisual.ToText();

            Controls.Add(ctl);
        }

        private void ctl_SelectedDateChanged(object sender, SelectedDateChangedEventArgs args)
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
