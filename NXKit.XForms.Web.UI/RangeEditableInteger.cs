using System;
using System.Diagnostics.Contracts;
using System.Web.UI.WebControls;

using Telerik.Web.UI;

namespace NXKit.XForms.Web.UI
{

    [XFormsXsdType(XmlSchemaConstants.XMLSchema_NS, "int")]
    [XFormsXsdType(XmlSchemaConstants.XMLSchema_NS, "integer")]
    public class RangeEditableInteger :
        RangeEditable
    {

        RadNumericTextBox ctl;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        public RangeEditableInteger(NXKit.Web.UI.View view, XFormsRangeVisual visual)
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
            ctl = new RadNumericTextBox();
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

            int start;
            if (int.TryParse(Visual.Start, out start))
                ctl.MinValue = start;

            int end;
            if (int.TryParse(Visual.End, out end))
                ctl.MaxValue = end;

            int step;
            if (int.TryParse(Visual.Step, out step))
                ctl.IncrementSettings.Step = step;

            Controls.Add(ctl);
        }

        void ctl_TextChanged(object sender, EventArgs args)
        {
            BindingUtil.Set(Visual.Binding, ctl.Value);
        }

        public override string TargetID
        {
            get { return ctl.ClientID; }
        }

        protected override void OnVisualValueChanged()
        {
            ctl.Value = BindingUtil.Get<double?>(Visual.Binding);
        }

        protected override void OnVisualReadOnlyOrReadWrite()
        {
            ctl.Enabled = !Visual.ReadOnly;
        }

    }

}