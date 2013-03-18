using System.Web.UI;
using NXKit.Web.UI;
using NXKit.XForms;

namespace NXKit.XForms.Web.UI
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

        public override VisualControl CreateControl(View view, Visual visual)
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
        public OutputControl(View view, XFormsOutputVisual visual)
            : base(view, visual)
        {

        }

        protected override void Render(HtmlTextWriter writer)
        {
            Visual.WriteText(writer);
        }

    }

}
