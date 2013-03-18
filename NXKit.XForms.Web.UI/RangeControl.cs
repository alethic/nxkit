using NXKit.Web.UI;
using NXKit.XForms;

namespace NXKit.XForms.Web.UI
{

    [VisualControlTypeDescriptor]
    public class RangeControlDescriptor : VisualControlTypeDescriptor
    {

        public override bool CanHandleVisual(Visual visual)
        {
            return visual is XFormsRangeVisual;
        }

        public override bool IsContent(Visual visual)
        {
            return true;
        }

        public override VisualControl CreateControl(View view, Visual visual)
        {
            return new RangeControl(view, (XFormsRangeVisual)visual);
        }

    }

    public class RangeControl : VisualControl<XFormsRangeVisual>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="visual"></param>
        public RangeControl(View view, XFormsRangeVisual visual)
            : base(view, visual)
        {

        }

        protected override void CreateChildControls()
        {
            var ctl = CreateRangeControl(Visual);
            ctl.ID = Visual.Type != null ? Visual.Type.LocalName : "default";
            Controls.Add(ctl);
        }

        /// <summary>
        /// Creates a Range control based on the bound data type.
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        private VisualControl<XFormsRangeVisual> CreateRangeControl(XFormsRangeVisual visual)
        {
            if (Visual.Type == XmlSchemaConstants.XMLSchema + "integer" ||
                Visual.Type == XmlSchemaConstants.XMLSchema + "int")
                return new RangeIntegerControl(View, Visual);
            else if (Visual.Type == XmlSchemaConstants.XMLSchema + "date")
                return new RangeDateControl(View, Visual);
            else
                return null;
        }

    }

}
