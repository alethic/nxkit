namespace NXKit.XForms
{

    public class XFormsInvalidEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-invalid";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsInvalidEvent()
            : base(Name, true, false)
        {

        }

    }

}
