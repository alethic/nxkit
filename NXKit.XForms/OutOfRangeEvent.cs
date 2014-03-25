namespace NXKit.XForms
{

    public class OutOfRangeEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-out-of-range";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public OutOfRangeEvent(NXNode visual)
            : base(visual, Name, true, false)
        {

        }

    }

}
