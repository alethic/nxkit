using System.Web.UI;

using XEngine.Forms.XForms;

namespace XEngine.Forms.Web.UI.XForms
{

    [VisualControlTypeDescriptor]
    public class LabelControlDescriptor : VisualControlTypeDescriptor
    {

        public override bool CanHandleVisual(Visual visual)
        {
            return visual is XFormsLabelVisual;
        }

        public override bool IsContent(Visual visual)
        {
            return false;
        }

        public override VisualControl CreateControl(FormView view, Visual visual)
        {
            return new LabelControl(view, (XFormsLabelVisual)visual);
        }

    }

    public class LabelControl : VisualContentControl<XFormsLabelVisual>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        public LabelControl(FormView view, XFormsLabelVisual visual)
            : base(view, visual)
        {

        }

        protected override VisualControlCollection CreateContentControlCollection()
        {
            return new VisualContentControlCollection(View, Visual, includeTextAsContent: true);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (Visual.Binding != null)
                // ignore child visual's if we have a binding
                writer.WriteEncodedText(Visual.Binding.Value ?? "");
            else
                base.Render(writer);
        }

    }

}
