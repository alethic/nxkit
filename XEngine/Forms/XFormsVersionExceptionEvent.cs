namespace XEngine.Forms.XForms
{

    public class XFormsVersionExceptionEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-version-exception";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsVersionExceptionEvent()
            : base(Name, true, false)
        {

        }

    }

}
