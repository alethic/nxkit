using System.Web.UI;

namespace NXKit.Web.UI
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

        public override VisualControl CreateControl(View view, Visual visual)
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
        public TextVisualControl(View view, TextVisual visual)
            : base(view, visual)
        {

        }

        protected override void Render(HtmlTextWriter writer)
        {
            Visual.WriteText(writer);
        }

    }

}
