using System.Linq;

using ISIS.Forms.XForms;

namespace ISIS.Forms.Web.UI.XForms
{

    public class SelectControl : VisualControl<XFormsSelectVisual>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="visual"></param>
        public SelectControl(FormView view, XFormsSelectVisual visual)
            : base(view, visual)
        {

        }

        protected override void CreateChildControls()
        {
            if (Visual.Appearance == Constants.XForms_1_0 + "full")
                CreateSelectFullControl();
            else if (Visual.Appearance == Constants.XForms_1_0 + "compact")
                CreateSelectCompactControl();
            else if (Visual.Appearance == Constants.XForms_1_0 + "minimal")
                CreateSelectMinimalControl();
        }

        private void CreateSelectFullControl()
        {
            var ctl = new SelectFullControl(View, Visual);
            ctl.ID = "full";
            Controls.Add(ctl);
        }

        private void CreateSelectCompactControl()
        {
            var ctl = new SelectCompactControl(View, Visual);
            ctl.ID = "compact";
            Controls.Add(ctl);
        }

        private void CreateSelectMinimalControl()
        {
            var ctl = new SelectMinimalControl(View, Visual);
            ctl.ID = "minimal";
            Controls.Add(ctl);
        }

    }

}
