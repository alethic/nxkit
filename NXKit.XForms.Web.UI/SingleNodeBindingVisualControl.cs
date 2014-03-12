using System;
using System.Diagnostics.Contracts;

using NXKit.Web.UI;

namespace NXKit.XForms.Web.UI
{

    /// <summary>
    /// Abstract base for <see cref="VisualControl"/> instances that associated with <see cref="XFormsSingleNodeBindingVisual"/>s.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SingleNodeBindingVisualControl<T> :
        VisualControl<T>
        where T : XFormsSingleNodeBindingVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        protected SingleNodeBindingVisualControl(View view, T visual)
            : base(view, visual)
        {
            Contract.Requires<ArgumentNullException>(view != null);
            Contract.Requires<ArgumentNullException>(visual != null);

            Visual.AddEventHandler<XFormsValueChangedEvent>(Visual_ValueChanged, false);
            Visual.AddEventHandler<XFormsEnabledEvent>(Visual_Enabled, false);
            Visual.AddEventHandler<XFormsDisabledEvent>(Visual_Disabled, false);
            Visual.AddEventHandler<XFormsRequiredEvent>(Visual_Required, false);
            Visual.AddEventHandler<XFormsOptionalEvent>(Visual_Optional, false);
            Visual.AddEventHandler<XFormsValidEvent>(Visual_Valid, false);
            Visual.AddEventHandler<XFormsInvalidEvent>(Visual_Invalid, false);
            Visual.AddEventHandler<XFormsReadOnlyEvent>(Visual_ReadOnly, false);
            Visual.AddEventHandler<XFormsReadWriteEvent>(Visual_ReadWrite, false);
        }

        void Visual_ValueChanged(Event evt)
        {
            OnVisualValueChanged();
        }

        /// <summary>
        /// Invoked when the underlying visual's value is changed.
        /// </summary>
        protected virtual void OnVisualValueChanged()
        {

        }

        void Visual_Enabled(Event evt)
        {
            OnVisualEnabled();
        }

        /// <summary>
        /// Invoked when the underlying visual is enabled.
        /// </summary>
        protected virtual void OnVisualEnabled()
        {
            OnVisualEnabledOrDisabled();
        }

        void Visual_Disabled(Event evt)
        {
            OnVisualDisabled();
        }

        /// <summary>
        /// Invoked when the underlying visual is disabled.
        /// </summary>
        protected virtual void OnVisualDisabled()
        {
            OnVisualEnabledOrDisabled();
        }

        /// <summary>
        /// Invoked when the underlying visual is enabled or disabled.
        /// </summary>
        protected virtual void OnVisualEnabledOrDisabled()
        {

        }

        void Visual_Required(Event evt)
        {
            OnVisualRequired();
        }

        /// <summary>
        /// Invoked when the underlying visual is required.
        /// </summary>
        protected virtual void OnVisualRequired()
        {
            OnVisualRequiredOrOptional();
        }

        void Visual_Optional(Event evt)
        {
            OnVisualOptional();
        }

        /// <summary>
        /// Invoked when the underlying visual is optional.
        /// </summary>
        protected virtual void OnVisualOptional()
        {
            OnVisualRequiredOrOptional();
        }

        /// <summary>
        /// Invoked when the underlying visual is made required or optional.
        /// </summary>
        protected virtual void OnVisualRequiredOrOptional()
        {

        }

        void Visual_Valid(Event evt)
        {
            OnVisualValid();
        }

        /// <summary>
        /// Invoked when the underlying visual is valid.
        /// </summary>
        protected virtual void OnVisualValid()
        {
            OnVisualValidOrInvalid();
        }

        void Visual_Invalid(Event evt)
        {
            OnVisualInvalid();
        }

        /// <summary>
        /// Invoked when the underlying visual is invalid.
        /// </summary>
        protected virtual void OnVisualInvalid()
        {
            OnVisualValidOrInvalid();
        }

        /// <summary>
        /// Invoked when the underlying visual is made valid or invalid.
        /// </summary>
        protected virtual void OnVisualValidOrInvalid()
        {

        }

        void Visual_ReadOnly(Event evt)
        {
            OnVisualReadOnly();
        }

        /// <summary>
        /// Invoked when the underlying visual is read-only.
        /// </summary>
        protected virtual void OnVisualReadOnly()
        {
            OnVisualReadOnlyOrReadWrite();
        }

        void Visual_ReadWrite(Event evt)
        {
            OnVisualReadWrite();
        }

        /// <summary>
        /// Invoked when the underlying visual is read-write.
        /// </summary>
        protected virtual void OnVisualReadWrite()
        {
            OnVisualReadOnlyOrReadWrite();
        }

        /// <summary>
        /// Invoked when the underlying visual is made read-write or read-only.
        /// </summary>
        protected virtual void OnVisualReadOnlyOrReadWrite()
        {

        }

    }

}
