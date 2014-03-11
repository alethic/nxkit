using System.Web.UI;

using NXKit.Web.UI;

namespace NXKit.XForms.Web.UI
{

    [VisualControlTypeDescriptor]
    public class HintControlDescriptor : 
        VisualControlTypeDescriptor
    {

        public override bool CanHandleVisual(Visual visual)
        {
            return visual is XFormsHintVisual;
        }

        public override bool IsContent(Visual visual)
        {
            return false;
        }

        public override VisualControl CreateControl(View view, Visual visual)
        {
            return new HintControl(view, (XFormsHintVisual)visual);
        }

    }

    public class HintControl : 
        VisualContentControl<XFormsHintVisual>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        public HintControl(View view, XFormsHintVisual visual)
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
