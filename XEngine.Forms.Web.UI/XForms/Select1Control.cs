using XEngine.Forms;

namespace XEngine.Forms.Web.UI.XForms
{

    [VisualControlTypeDescriptor]
    public class Select1ControlDescriptor : VisualControlTypeDescriptor
    {

        public override bool CanHandleVisual(Visual visual)
        {
            return visual is XFormsSelect1Visual;
        }

        public override bool IsContent(Visual visual)
        {
            return true;
        }

        public override VisualControl CreateControl(FormView view, Visual visual)
        {
            return new Select1Control(view, (XFormsSelect1Visual)visual);
        }

    }

    public class Select1Control : VisualControl<XFormsSelect1Visual>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="visual"></param>
        public Select1Control(FormView view, XFormsSelect1Visual visual)
            : base(view, visual)
        {

        }

        protected override void CreateChildControls()
        {
            if (Visual.Appearance == Constants.XForms_1_0 + "full")
                CreateSelect1FullControl();
            else if (Visual.Appearance == Constants.XForms_1_0 + "compact")
                CreateSelect1CompactControl();
            else if (Visual.Appearance == Constants.XForms_1_0 + "minimal")
                CreateSelect1MinimalControl();
            else
                CreateSelect1MinimalControl();
        }

        private void CreateSelect1FullControl()
        {
            var ctl = new Select1FullControl(View, Visual);
            ctl.ID = "full";
            Controls.Add(ctl);
        }

        private void CreateSelect1CompactControl()
        {
            var ctl = new Select1CompactControl(View, Visual);
            ctl.ID = "compact";
            Controls.Add(ctl);
        }

        private void CreateSelect1MinimalControl()
        {
            var ctl = new Select1MinimalControl(View, Visual);
            ctl.ID = "minimal";
            Controls.Add(ctl);
        }

    }

}
