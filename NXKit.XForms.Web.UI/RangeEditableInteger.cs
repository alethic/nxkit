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

        TextBox ctl;

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

        public override string TargetID
        {
            get { return ctl.ClientID; }
        }

        /// <summary>
        /// Creates the child control hierarchy.
        /// </summary>
        protected override void CreateChildControls()
        {
            ctl = new TextBox();
            ctl.ValidationGroup = View.ValidationGroup;
            ctl.ID = "ctl";
            ctl.Text = ((int?)BindingUtil.Get<double?>(Visual.Binding)).ToString();
            ctl.TextMode = TextBoxMode.Number;
            ctl.TextChanged += ctl_TextChanged;
            Controls.Add(ctl);

            int start;
            if (int.TryParse(Visual.Start, out start))
                ctl.Attributes["min"] = start.ToString();

            int end;
            if (int.TryParse(Visual.End, out end))
                ctl.Attributes["max"] = end.ToString();

            int step;
            if (int.TryParse(Visual.Step, out step))
                ctl.Attributes["step"] = step.ToString();
        }

        void ctl_TextChanged(object sender, EventArgs args)
        {
            int i;
            if (int.TryParse(ctl.Text, out i))
                BindingUtil.Set(Visual.Binding, (double?)i);
            else
                BindingUtil.Set(Visual.Binding, null);
        }

        protected override void OnVisualValueChanged()
        {
            ctl.Text = ((int?)BindingUtil.Get<double?>(Visual.Binding)).ToString();
        }

        protected override void OnVisualReadOnlyOrReadWrite()
        {
            ctl.Enabled = !Visual.ReadOnly;
        }

    }

}