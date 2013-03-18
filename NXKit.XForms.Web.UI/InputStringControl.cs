﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

using Telerik.Web.UI;

using NXKit.XForms;
using System.Web.UI.WebControls;
using NXKit.Web.UI;

namespace NXKit.XForms.Web.UI
{

    public class InputStringControl : VisualControl<XFormsInputVisual>, IScriptControl
    {

        private RadTextBox ctl;
        private CustomValidator val;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="visual"></param>
        public InputStringControl(NXKit.Web.UI.FormView view, XFormsInputVisual visual)
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
            ctl = new RadTextBox();
            ctl.ID = "ctl";
            ctl.EnableViewState = true;
            ctl.TextChanged += ctl_TextChanged;

            var labelVisual = Visual.Children.OfType<XFormsLabelVisual>().FirstOrDefault();
            if (labelVisual != null)
                ctl.EmptyMessage = labelVisual.ToText();

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
                    val.ErrorMessage = alertVisual.ToText();
            }
        }

        private void ctl_TextChanged(object sender, EventArgs args)
        {
            BindingUtil.Set(Visual.Binding, ctl.Text);
        }

        protected override void OnPreRender(EventArgs args)
        {
            base.OnPreRender(args);
            ScriptManager.GetCurrent(Page).RegisterScriptControl(this);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            ScriptManager.GetCurrent(Page).RegisterScriptDescriptors(this);

            // client-side control element
            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "XForms_Input XForms_Input_String");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            base.Render(writer);
            writer.RenderEndTag();
        }

        IEnumerable<ScriptDescriptor> IScriptControl.GetScriptDescriptors()
        {
            var desc = new ScriptControlDescriptor("NXKit.XForms.Web.UI.InputStringControl", ClientID);
            desc.AddComponentProperty("formView", View.ClientID);
            desc.AddProperty("modelItemId", Visual.Binding != null ? Visual.Binding.NodeUniqueId : null);
            desc.AddComponentProperty("radTextBox", ctl.ClientID);
            yield return desc;
        }

        IEnumerable<ScriptReference> IScriptControl.GetScriptReferences()
        {
            yield return new ScriptReference("NXKit.XForms.Web.UI.InputStringControl.js", typeof(InputStringControl).Assembly.FullName);
        }

    }

}