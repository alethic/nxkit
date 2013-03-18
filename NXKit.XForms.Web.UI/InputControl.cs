using NXKit.Web.UI;

namespace NXKit.XForms.Web.UI
{

    [VisualControlTypeDescriptor]
    public class InputControlDescriptor : VisualControlTypeDescriptor
    {

        public override bool CanHandleVisual(Visual visual)
        {
            return visual is XFormsInputVisual;
        }

        public override bool IsContent(Visual visual)
        {
            return true;
        }

        public override VisualControl CreateControl(FormView view, Visual visual)
        {
            return new InputControl(view, (XFormsInputVisual)visual);
        }

    }

    public class InputControl : VisualControl<XFormsInputVisual>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="visual"></param>
        public InputControl(FormView view, XFormsInputVisual visual)
            : base(view, visual)
        {

        }

        protected override void CreateChildControls()
        {
            var ctl = CreateInputControl(Visual);
            ctl.ID = Visual.Type != null ? Visual.Type.LocalName : "default";
            Controls.Add(ctl);
        }

        /// <summary>
        /// Creates an input control based on the bound data type.
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        private VisualControl<XFormsInputVisual> CreateInputControl(XFormsInputVisual visual)
        {
            if (Visual.Type == XmlSchemaConstants.XMLSchema + "boolean")
                return new InputBooleanControl(View, Visual);
            else if (Visual.Type == XmlSchemaConstants.XMLSchema + "date")
                return new InputDateControl(View, Visual);
            else if (Visual.Type == XmlSchemaConstants.XMLSchema + "time")
                return new InputTimeControl(View, Visual);
            else if (Visual.Type == XmlSchemaConstants.XMLSchema + "int")
                return new InputIntegerControl(View, Visual);
            else if (Visual.Type == XmlSchemaConstants.XMLSchema + "integer")
                return new InputIntegerControl(View, Visual);
            else if (Visual.Type == XmlSchemaConstants.XMLSchema + "long")
                return new InputIntegerControl(View, Visual);
            else if (Visual.Type == XmlSchemaConstants.XMLSchema + "short")
                return new InputIntegerControl(View, Visual);
            else
                return new InputStringControl(View, Visual);
        }

    }

}
