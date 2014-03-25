namespace NXKit.XForms
{

    public class SubmitDoneEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-submit-done";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public SubmitDoneEvent(NXNode visual)
            : base(visual, Name, true, false)
        {

        }

    }

}
