using System;
using System.Diagnostics.Contracts;
using System.Web.UI.WebControls;

namespace NXKit.XForms.Web.UI.Input
{

    [XFormsXsdType(XmlSchemaConstants.XMLSchema_NS, "boolean")]
    public class InputEditableBoolean :
        InputEditable
    {

        CheckBox ctl;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        public InputEditableBoolean(NXKit.Web.UI.View view, XFormsInputVisual visual)
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
            ctl = new CheckBox();
            ctl.ID = "ctl";
            ctl.AutoPostBack = Visual.Incremental();
            ctl.CheckedChanged += ctl_CheckedChanged;
            Controls.Add(ctl);
        }

        void ctl_CheckedChanged(object sender, EventArgs args)
        {
            BindingUtil.Set(Visual.Binding, ctl.Checked);
        }

        protected override void OnVisualValueChanged()
        {
            ctl.Checked = BindingUtil.Get<bool?>(Visual.Binding) ?? false;
        }

        protected override void OnVisualReadOnlyOrReadWrite()
        {
            ctl.Enabled = !Visual.ReadOnly;
        }

        //protected override void Render(System.Web.UI.HtmlTextWriter writer)
        //{
        //    //writer.AddAttribute(HtmlTextWriterAttribute.Class, "XForms_Input XForms_Input__boolean");
        //    //writer.RenderBeginTag(HtmlTextWriterTag.Div);
        //    //base.Render(writer);
        //    //writer.RenderEndTag();
        //}

    }

}
