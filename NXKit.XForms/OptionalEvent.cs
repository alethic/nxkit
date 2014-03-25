namespace NXKit.XForms
{

    public class OptionalEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-optional";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public OptionalEvent(NXNode visual)
            : base(visual,Name, true, false)
        {

        }

    }

}
