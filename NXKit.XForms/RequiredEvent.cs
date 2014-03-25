namespace NXKit.XForms
{

    public class RequiredEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-required";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public RequiredEvent(NXNode visual)
            : base(visual, Name, true, false)
        {

        }

    }

}
