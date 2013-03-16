using System.Web.UI;

namespace XEngine.Forms.Web.UI
{

    [VisualControlTypeDescriptor]
    public class TextVisualControlDescriptor : VisualControlTypeDescriptor
    {

        public override bool CanHandleVisual(Visual visual)
        {
            return visual is TextVisual;
        }

        public override bool IsContent(Visual visual)
        {
            return true;
        }

        public override VisualControl CreateControl(FormView view, Visual visual)
        {
            return new TextVisualControl(view, (TextVisual)visual);
        }

    }

    public class TextVisualControl : VisualControl<TextVisual>
    {
        
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        public TextVisualControl(FormView view, TextVisual visual)
            : base(view, visual)
        {

        }

        protected override void Render(HtmlTextWriter writer)
        {
            FormHelper.RenderText(writer, Visual);
        }

    }

}
