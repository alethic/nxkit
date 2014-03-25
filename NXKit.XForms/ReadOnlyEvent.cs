namespace NXKit.XForms
{

    public class ReadOnlyEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-readonly";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ReadOnlyEvent(NXNode visual)
            : base(visual, Name, true, false)
        {

        }

    }

}
