namespace NXKit.XForms
{

    public class XFormsRecalculateEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-recalculate";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsRecalculateEvent(NXNode visual)
            : base(visual, Name, true, true)
        {

        }

    }

}
