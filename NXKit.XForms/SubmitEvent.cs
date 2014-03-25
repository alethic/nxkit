namespace NXKit.XForms
{

    public class SubmitEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-submit";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public SubmitEvent(NXNode visual)
            : base(visual, Name, true, true)
        {

        }

    }
}
