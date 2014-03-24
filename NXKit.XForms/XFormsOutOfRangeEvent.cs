namespace NXKit.XForms
{

    public class XFormsOutOfRangeEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-out-of-range";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsOutOfRangeEvent(NXNode visual)
            : base(visual, Name, true, false)
        {

        }

    }

}
