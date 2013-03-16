using System.Web.UI;

using XEngine.Forms.XForms;

namespace XEngine.Forms.Web.UI.XForms
{

    [VisualControlTypeDescriptor]
    public class OutputControlDescriptor : VisualControlTypeDescriptor
    {

        public override bool CanHandleVisual(Visual visual)
        {
            return visual is XFormsOutputVisual;
        }

        public override bool IsContent(Visual visual)
        {
            return true;
        }

        public override VisualControl CreateControl(FormView view, Visual visual)
        {
            return new OutputControl(view, (XFormsOutputVisual)visual);
        }

    }

    public class OutputControl : VisualControl<XFormsOutputVisual>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        public OutputControl(FormView view, XFormsOutputVisual visual)
            : base(view, visual)
        {

        }

        protected override void Render(HtmlTextWriter writer)
        {
            FormHelper.RenderOutput(writer, Visual);
        }

    }

}
