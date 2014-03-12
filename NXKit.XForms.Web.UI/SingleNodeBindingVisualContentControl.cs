using System;
using System.Diagnostics.Contracts;
using System.Web.UI;

using NXKit.Web.UI;

namespace NXKit.XForms.Web.UI
{

    /// <summary>
    /// Abstract base for <see cref="VisualControl"/> instances that associated with <see cref="XFormsSingleNodeBindingVisual"/>s.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SingleNodeBindingVisualContentControl<T> :
        SingleNodeBindingVisualControl<T>
        where T : XFormsSingleNodeBindingVisual
    {

        VisualControlCollection content;

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
        }

        /// <summary>
        /// Contains child visual controls.
        /// </summary>
        protected VisualControlCollection Content
        {
            get { return content; }
        }

        /// <summary>
        /// Creates the <see cref="VisualContentControlCollection"/>.
        /// </summary>
        protected virtual VisualControlCollection CreateContentControlCollection()
        {
            return new VisualContentControlCollection(View, Visual);
        }

        /// <summary>
        /// Creates the control hierarchy.
        /// </summary>
        protected override void CreateChildControls()
        {
            Controls.Clear();
            Controls.Add(content = CreateContentControlCollection());
        }

        /// <summary>
        /// Writes the HTML for the control to the output.
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            Content.RenderControl(writer);
        }

    }

}
