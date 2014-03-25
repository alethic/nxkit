namespace NXKit.XForms
{

    public class InvalidEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-invalid";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public InvalidEvent(NXNode visual)
            : base(visual, Name, true, false)
        {

        }

    }

}
