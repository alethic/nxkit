namespace XEngine.Forms.XForms
{

    public class XFormsRecalculateEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-recalculate";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsRecalculateEvent()
            : base(Name, true, true)
        {

        }

    }

}
