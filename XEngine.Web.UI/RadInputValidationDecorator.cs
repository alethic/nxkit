using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using Telerik.Web.UI;

namespace XEngine.Forms.Web.UI
{

    /// <summary>
    /// Ensures the 'invalid' state is set for a RadInput based on both client and server-side validation.
    /// </summary>
    public class RadInputValidationDecorator : Control, IScriptControl
    {

        /// <summary>
        /// Inserts itself into the validation framework.
        /// </summary>
        private class _RadInputValidator : CustomValidator
        {

            private RadInputValidationDecorator decorator;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="decorator"></param>
            public _RadInputValidator(RadInputValidationDecorator decorator)
            {
                this.decorator = decorator;
            }

            /// <summary>
            /// Invoked for the Init phase.
            /// </summary>
            /// <param name="args"></param>
            protected override void OnInit(EventArgs args)
            {
                base.OnInit(args);

                EnableViewState = false;
                ControlToValidate = decorator.TargetControlID;
                ValidationGroup = decorator.ValidationGroup;
                Display = ValidatorDisplay.None;
                ValidateEmptyText = true;
                ClientValidationFunction = "ISIS.Forms.Web.UI.RadValidationDecorator.RadInputValidationDecorator_ValidationFunction";
            }

        }

        private _RadInputValidator validator;

        /// <summary>
        /// Should we inject JS that changes the associated target to invalid?
        /// </summary>
        private bool invalid;

        /// <summary>
        /// Gets or sets the ID of the RadInput control whose state will be updated.
        /// </summary>
        public string TargetControlID { get; set; }

        /// <summary>
        /// Gets or sets the validation group to attach state modification to.
        /// </summary>
        public string ValidationGroup { get; set; }

        /// <summary>
        /// Invoked for the Init phase.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnInit(EventArgs args)
        {
            base.OnInit(args);

            EnsureChildControls();
        }

        /// <summary>
        /// Creates the child control hierarchy.
        /// </summary>
        protected override void CreateChildControls()
        {
            validator = new _RadInputValidator(this);
            validator.ServerValidate += validator_ServerValidate;
            Controls.Add(validator);
        }

        /// <summary>
        /// Invoked to validate server side.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        private void validator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;

            invalid = false;

            // find our target
            var ctl = FindControl(TargetControlID);

            // validators pointing to same control, that are currently invalid
            var validators = Page.GetValidators(ValidationGroup)
                .OfType<BaseValidator>()
                .Where(i => !string.IsNullOrEmpty(i.ControlToValidate))
                .Where(i => !i.IsValid)
                .Where(i => i.FindControl(i.ControlToValidate) == ctl);

            // refresh validators
            foreach (var validator in validators)
                validator.Validate();

            if (validators.Any(i => !i.IsValid))
                invalid = true;
        }

        /// <summary>
        /// Invoked for the PreRender phase.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnPreRender(EventArgs args)
        {
            base.OnPreRender(args);

            // register script control
            ScriptManager.GetCurrent(Page).RegisterScriptControl(this);

            var ctl = FindControl(TargetControlID) as RadInputControl;
            if (ctl == null)
                throw new Exception("Could not locate RadInput control by TargetControlID.");

            // if invalid, render a startup script block that registers a load handler to invalidate the control
            if (invalid)
                Page.ClientScript.RegisterStartupScript(typeof(RadInputControl), Guid.NewGuid().ToString(), @"
                    <script type=""text/javascript"">
                        Sys.Application.add_load(function()
                        {
                            var ctl = $find(""" + ctl.ClientID + @""");
                            ISIS.Forms.Web.UI.RadValidationDecorator.RadInputValidationDecorator_SetInvalid(ctl, true);
                        });
                    </script>");
        }

        IEnumerable<ScriptReference> IScriptControl.GetScriptReferences()
        {
            yield return new ScriptReference("ISIS.Forms.Web.UI.RadValidationDecorator.js", "ISIS.Forms.Web.UI");
        }

        IEnumerable<ScriptDescriptor> IScriptControl.GetScriptDescriptors()
        {
            yield break;
        }

    }

}
