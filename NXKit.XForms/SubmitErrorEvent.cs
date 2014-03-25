namespace NXKit.XForms
{

    public class SubmitErrorEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-submit-error";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public SubmitErrorEvent(NXNode visual)
            : base(visual, Name, true, false)
        {

        }

    }

}
