using System;
using System.Diagnostics.Contracts;
using System.Web.UI;

namespace NXKit.Web.UI
{

    /// <summary>
    /// Base <see cref="VisualControl"/> type to assist with <see cref="Visual"/> instances that contain further content.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class VisualContentControl<T> :
        VisualControl<T>
        where T : StructuralVisual
    {

        VisualControlCollection content;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        public VisualContentControl(View view, T visual)
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
