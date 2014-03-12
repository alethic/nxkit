using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public override VisualControl CreateControl(View view, Visual visual)
        {
            return new InputControl(view, (XFormsInputVisual)visual);
        }

    }

}
