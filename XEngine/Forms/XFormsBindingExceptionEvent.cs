namespace XEngine.Forms.XForms
{

    public class XFormsBindingExceptionEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-binding-exception";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsBindingExceptionEvent()
            : base(Name, true, false)
        {

        }

    }

}
