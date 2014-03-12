using NXKit.Web.UI;

namespace NXKit.XForms.Web.UI
{

    [VisualControlTypeDescriptor]
    public class RepeatControlDescriptor :
        VisualControlTypeDescriptor
    {

        public override bool CanHandleVisual(Visual visual)
        {
            return
                visual is XFormsRepeatVisual ||
                visual is XFormsRepeatItemVisual;
        }

        public override bool IsContent(Visual visual)
        {
            return true;
        }

        public override bool IsOpaque(Visual visual)
        {
            return false;
        }

    }

}
