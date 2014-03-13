using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.UI.WebControls;

using Telerik.Web.UI;

using NXKit.Web.UI;

namespace NXKit.XForms.Web.UI
{

    [XFormsXsdType(XmlSchemaConstants.XMLSchema_NS, "integer")]
    [XFormsXsdType(XmlSchemaConstants.XMLSchema_NS, "int")]
    public class InputEditableInteger :
        InputEditable
    {

        RadNumericTextBox ctl;
        CustomValidator val;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="visual"></param>
        public InputEditableInteger(NXKit.Web.UI.View view, XFormsInputVisual visual)
            : base(view, visual)
        {
            Contract.Requires<ArgumentNullException>(view != null);
            Contract.Requires<ArgumentNullException>(visual != null);
        }

        public override string TargetID
        {
            get { return ctl.ClientID; }
        }

        protected override void OnVisualValueChanged()
        {
            base.OnVisualValueChanged();
            ctl.Text = BindingUtil.Get<string>(Visual.Binding);
        }

        protected override void OnVisualReadOnlyOrReadWrite()
        {
            base.OnVisualReadOnlyOrReadWrite();
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

            var labelVisual = Visual.FindLabelVisual();
            if (labelVisual != null)
                ctl.Label = labelVisual.ToText();

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

        void ctl_TextChanged(object sender, EventArgs args)
        {
            BindingUtil.Set(Visual.Binding, ctl.Value);
        }

        void validator_ServerValidate(object source, ServerValidateEventArgs args)
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
                    val.ErrorMessage = alertVisual.ToText();
            }
        }

    }

}