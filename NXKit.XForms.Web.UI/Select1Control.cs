using System;
using System.Diagnostics.Contracts;

using NXKit.Web.UI;

namespace NXKit.XForms.Web.UI
{

    [VisualControlTypeDescriptor]
    public class Select1ControlDescriptor :
        VisualControlTypeDescriptor
    {

        public override bool CanHandleVisual(Visual visual)
        {
            return visual is XFormsSelect1Visual;
        }

        public override bool IsContent(Visual visual)
        {
            return true;
        }

        public override VisualControl CreateControl(View view, Visual visual)
        {
            return new Select1Control(view, (XFormsSelect1Visual)visual);
        }

    }

    public class Select1Control :
        SingleNodeBindingVisualControl<XFormsSelect1Visual>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        public Select1Control(View view, XFormsSelect1Visual visual)
            : base(view, visual)
        {
            Contract.Requires<ArgumentNullException>(view != null);
            Contract.Requires<ArgumentNullException>(visual != null);
        }

        protected override void CreateChildControls()
        {
            var appearance = Visual.Appearance();

            if (appearance == Constants.XForms_1_0 + "full")
                CreateSelect1FullControl();
            else if (appearance == Constants.XForms_1_0 + "compact")
                CreateSelect1CompactControl();
            else if (appearance == Constants.XForms_1_0 + "minimal")
                CreateSelect1MinimalControl();
            else
                CreateSelect1MinimalControl();
        }

        void CreateSelect1FullControl()
        {
            var ctl = new Select1FullControl(View, Visual);
            ctl.ID = "full";
            Controls.Add(ctl);
        }

        void CreateSelect1CompactControl()
        {
            var ctl = new Select1CompactControl(View, Visual);
            ctl.ID = "compact";
            Controls.Add(ctl);
        }

        void CreateSelect1MinimalControl()
        {
            var ctl = new Select1MinimalControl(View, Visual);
            ctl.ID = "minimal";
            Controls.Add(ctl);
        }

    }

}
