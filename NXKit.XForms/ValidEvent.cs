namespace NXKit.XForms
{

    public class ValidEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-valid";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ValidEvent(NXNode visual)
            : base(visual, Name, true, false)
        {

        }

    }

}
