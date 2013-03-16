namespace XEngine.Forms.XForms
{

    public class XFormsOutOfRangeEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-out-of-range";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsOutOfRangeEvent()
            : base(Name, true, false)
        {

        }

    }

}
