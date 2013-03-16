using System;
using System.Linq;
using System.Web.UI.WebControls;

using Telerik.Web.UI;

using ISIS.Forms.XForms;

namespace ISIS.Forms.Web.UI.XForms
{

    public class InputIntegerControl : VisualControl<XFormsInputVisual>
    {

        private RadNumericTextBox ctl;
        private CustomValidator val;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="visual"></param>
        public InputIntegerControl(FormView view, XFormsInputVisual visual)
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

            ctl.Text = BindingUtil.Get<string>(Visual.Binding);
        }

        /// <summary>
        /// Invoked when the read-only state of the Visual changes.
        /// </summary>
        /// <param name="ev"></param>
        private void Visual_ReadOnlyChanged(Event ev)
        {
            EnsureChildControls();

            ctl.ReadOnly = Visual.ReadOnly;
        }

        /// <summary>
        /// Creates the child control hierarchy.
        /// </summary>
        protected override void CreateChildControls()
        {
            ctl = new RadNumericTextBox();
            ctl.ValidationGroup = View.ValidationGroup;
            ctl.ID = "ctl";
            ctl.Value = BindingUtil.Get<double?>(Visual.Binding);
            ctl.Type = NumericType.Percent;
            ctl.Width = Unit.Pixel(150);
            ctl.ShowSpinButtons = true;
            ctl.IncrementSettings.Step = 5;
            ctl.NumberFormat.DecimalDigits = 0;
            ctl.NumberFormat.NegativePattern = "-n";
            ctl.NumberFormat.PositivePattern = "n";
            ctl.NumberFormat.GroupSeparator = "";
            ctl.TextChanged += ctl_TextChanged;

            var labelVisual = Visual.Children.OfType<XFormsLabelVisual>().FirstOrDefault();
            if (labelVisual != null)
                ctl.Label = FormHelper.LabelToString(labelVisual);

            Controls.Add(ctl);

            val = new CustomValidator();
            val.ValidationGroup = View.ValidationGroup;
            val.ControlToValidate = ctl.ID;
            val.ValidateEmptyText = true;
            val.ServerValidate += validator_ServerValidate;
            val.Text = "";
            val.Display = ValidatorDisplay.None;
            Controls.Add(val);

            var decorator = new RadInputValidationDecorator();
            decorator.ValidationGroup = View.ValidationGroup;
            decorator.TargetControlID = ctl.ID;
            Controls.Add(decorator);
        }

        private void validator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            // skip irrelevant controls; can't be invalid
            if (!Visual.Relevant)
                return;

            // set validity based on binding
            if (args.IsValid)
                if (Visual.Binding != null && !Visual.Binding.Valid)
                    args.IsValid = false;

            // also invalid if required but empty
            if (args.IsValid)
                if (Visual.Binding != null && Visual.Binding.Required)
                    if (string.IsNullOrEmpty(Visual.Binding.Value))
                        args.IsValid = false;

            if (!args.IsValid)
            {
                var alertVisual = Visual.Children.OfType<XFormsAlertVisual>().FirstOrDefault();
                if (alertVisual != null)
                    val.ErrorMessage = FormHelper.AlertToString(alertVisual);
            }
        }

        private void ctl_TextChanged(object sender, EventArgs args)
        {
            BindingUtil.Set(Visual.Binding, ctl.Value);
        }

    }

}