namespace NXKit.XForms
{

    public class ReadyEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-ready";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ReadyEvent(NXNode visual)
            : base(visual, Name, true, false)
        {

        }

    }

}
