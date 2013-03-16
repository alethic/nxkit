using System.Web.UI;

namespace XEngine.Forms.Web.UI
{

    public class VisualContentControl<T> : VisualControl<T>
         where T : StructuralVisual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        public VisualContentControl(FormView view, T visual)
            : base(view, visual)
        {

        }

        /// <summary>
        /// Contains child visual controls.
        /// </summary>
        protected VisualControlCollection Content { get; private set; }

        /// <summary>
        /// Creates the <see cref="VisualContentControlCollection"/>.
        /// </summary>
        protected virtual VisualControlCollection CreateContentControlCollection()
        {
            return new VisualContentControlCollection(View, Visual);
        }

        /// <summary>
        /// Creates the child control hierarchy.
        /// </summary>
        protected override void CreateChildControls()
        {
            Controls.Clear();

            // recreate container
            Controls.Add(Content = CreateContentControlCollection());
        }

        protected override void Render(HtmlTextWriter writer)
        {
            EnsureChildControls();

            // render only visual content container by default
            Content.RenderControl(writer);
        }

    }

}
