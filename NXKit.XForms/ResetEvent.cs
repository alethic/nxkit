namespace NXKit.XForms
{

    public class ResetEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-reset";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ResetEvent(NXNode visual)
            : base(visual, Name, true, false)
        {

        }

    }

}
