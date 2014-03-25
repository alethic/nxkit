namespace NXKit.XForms
{

    public class EnabledEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-enabled";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public EnabledEvent(NXNode visual)
            : base(visual, Name, true, false)
        {

        }

    }

}
