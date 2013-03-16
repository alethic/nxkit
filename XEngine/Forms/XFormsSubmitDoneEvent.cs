namespace XEngine.Forms
{

    public class XFormsSubmitDoneEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-submit-done";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsSubmitDoneEvent()
            : base(Name, true, false)
        {

        }

    }

}
