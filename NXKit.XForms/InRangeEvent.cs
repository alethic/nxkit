namespace NXKit.XForms
{

    public class InRangeEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-in-range";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public InRangeEvent(NXNode visual)
            : base(visual, Name, true, false)
        {

        }

    }

}
