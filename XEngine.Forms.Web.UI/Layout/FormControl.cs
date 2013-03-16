using System.Web.UI;

using XEngine.Layout;

namespace XEngine.Forms.Web.UI.Layout
{

    [VisualControlTypeDescriptor]
    public class FormControlDescriptor : VisualControlTypeDescriptor
    {

        public override bool CanHandleVisual(Visual visual)
        {
            return visual is FormVisual;
        }

        public override bool IsContent(Visual visual)
        {
            return true;
        }

        public override VisualControl CreateControl(FormView view, Visual visual)
        {
            return new FormControl(view, (FormVisual)visual);
        }

    }

    public class FormControl : VisualContentControl<FormVisual>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="visual"></param>
        public FormControl(FormView view, FormVisual visual)
            : base(view, visual)
        {

        }

    }

}
