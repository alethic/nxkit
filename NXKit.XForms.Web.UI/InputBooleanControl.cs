using System;
using System.Web.UI.WebControls;

using NXKit.Web.UI;

namespace NXKit.XForms.Web.UI
{

    public class InputBooleanControl : VisualControl<XFormsInputVisual>
    {

        private CheckBox ctl;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="visual"></param>
        public InputBooleanControl(NXKit.Web.UI.View view, XFormsInputVisual visual)
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

            ctl.Checked = BindingUtil.Get<bool?>(Visual.Binding) ?? false;
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
            ctl = new CheckBox();
            ctl.ID = "ctl";
            ctl.AutoPostBack = Visual.Incremental();
            ctl.CheckedChanged += ctl_CheckedChanged;
            Controls.Add(ctl);
        }

        private void ctl_CheckedChanged(object sender, EventArgs args)
        {
            BindingUtil.Set(Visual.Binding, ctl.Checked);
        }

    }

}
