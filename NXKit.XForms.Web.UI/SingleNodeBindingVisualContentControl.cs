using System;
using System.Diagnostics.Contracts;

using NXKit.Web.UI;

namespace NXKit.XForms.Web.UI
{

    /// <summary>
    /// Abstract base for <see cref="VisualControl"/> instances that associated with <see cref="XFormsSingleNodeBindingVisual"/>s.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SingleNodeBindingVisualContentControl<T> :
        VisualContentControl<T>
        where T : XFormsSingleNodeBindingVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        protected SingleNodeBindingVisualContentControl(View view, T visual)
            : base(view, visual)
        {
            Contract.Requires<ArgumentNullException>(view != null);
            Contract.Requires<ArgumentNullException>(visual != null);

            visual.AddEventHandler<XFormsEnabledEvent>(Visual_Enabled, false);
            visual.AddEventHandler<XFormsDisabledEvent>(Visual_Disabled, false);
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

        }

    }

}
