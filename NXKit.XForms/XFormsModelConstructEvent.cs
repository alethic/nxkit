namespace NXKit.XForms
{

    public class XFormsModelConstructEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-model-construct";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsModelConstructEvent(NXNode visual)
            : base(visual, Name, true, false)
        {

        }

    }

}
