namespace ISIS.Forms.XForms
{

    public class XFormsModelConstructEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-model-construct";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsModelConstructEvent()
            : base(Name, true, false)
        {

        }

    }

}
