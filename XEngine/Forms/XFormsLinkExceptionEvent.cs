namespace XEngine.Forms.XForms
{

    public class XFormsLinkExceptionEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-link-exception";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsLinkExceptionEvent()
            : base(Name, true, false)
        {

        }

    }

}
