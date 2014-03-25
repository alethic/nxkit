namespace NXKit.XForms
{

    public class RecalculateEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-recalculate";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public RecalculateEvent(NXNode visual)
            : base(visual, Name, true, true)
        {

        }

    }

}
