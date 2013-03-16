namespace XEngine.Forms.XForms
{

    public class XFormsEnabledEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-enabled";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsEnabledEvent()
            : base(Name, true, false)
        {

        }

    }

}
