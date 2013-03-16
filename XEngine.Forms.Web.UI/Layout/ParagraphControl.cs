using System.Web.UI;

using XEngine.Forms.Layout;

namespace XEngine.Forms.Web.UI.Layout
{

    [VisualControlTypeDescriptor]
    public class ParagraphControlDescriptor : VisualControlTypeDescriptor
    {

        public override bool CanHandleVisual(Visual visual)
        {
            return visual is ParagraphVisual;
        }

        public override bool IsContent(Visual visual)
        {
            return true;
        }

        public override VisualControl CreateControl(FormView view, Visual visual)
        {
            return new ParagraphControl(view, (ParagraphVisual)visual);
        }

    }

    public class ParagraphControl : VisualContentControl<ParagraphVisual>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="visual"></param>
        public ParagraphControl(FormView view, ParagraphVisual visual)
            : base(view, visual)
        {

        }

        protected override VisualControlCollection CreateContentControlCollection()
        {
            return new VisualContentControlCollection(View, Visual, includeTextAsContent: true);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "Layout_Paragraph");
            writer.RenderBeginTag(HtmlTextWriterTag.P);
            base.Render(writer);
            writer.RenderEndTag();
        }

    }

}
