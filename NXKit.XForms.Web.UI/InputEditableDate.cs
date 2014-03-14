using System;
using System.Diagnostics.Contracts;
using System.Web.UI.WebControls;

namespace NXKit.XForms.Web.UI
{

    [XFormsXsdType(XmlSchemaConstants.XMLSchema_NS, "date")]
    public class InputEditableDate :
        InputEditable
    {

        TextBox ctl;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        public InputEditableDate(NXKit.Web.UI.View view, XFormsInputVisual visual)
            : base(view, visual)
        {
            Contract.Requires<ArgumentNullException>(view != null);
            Contract.Requires<ArgumentNullException>(visual != null);
        }

        protected override void OnVisualValueChanged()
        {
            ctl.Text = Convert.ToString(BindingUtil.Get<DateTime?>(Visual.Binding));
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
            ctl = new TextBox();
            ctl.ID = "ctl";
            ctl.TextMode = TextBoxMode.Date;
            ctl.Attributes["min"] = DateTime.MinValue.ToShortDateString();
            ctl.Attributes["max"] = DateTime.MaxValue.ToShortDateString();
            ctl.TextChanged += ctl_TextChanged;
            Controls.Add(ctl);
        }

        void ctl_TextChanged(object sender, EventArgs args)
        {
            DateTime d;
            if (DateTime.TryParse(ctl.Text, out d))
                BindingUtil.Set(Visual.Binding, d);
            else
                BindingUtil.Set(Visual.Binding, null);
        }

    }

}
