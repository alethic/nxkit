namespace ISIS.Forms.XForms
{

    public class XFormsRequiredEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-required";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsRequiredEvent()
            : base(Name, true, false)
        {

        }

    }

}
