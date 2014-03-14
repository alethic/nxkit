using System;
using System.Diagnostics.Contracts;
using System.Web.UI.WebControls;

using NXKit.Web.UI;

namespace NXKit.XForms.Web.UI
{

    [XFormsXsdType(XmlSchemaConstants.XMLSchema_NS, "integer")]
    [XFormsXsdType(XmlSchemaConstants.XMLSchema_NS, "int")]
    public class InputEditableInteger :
        InputEditable
    {

        TextBox ctl;

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
            ctl.ReadOnly = Visual.ReadOnly;
        }

    }

}