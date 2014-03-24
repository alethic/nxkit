namespace NXKit.XForms
{

    public class XFormsSubmitDoneEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-submit-done";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsSubmitDoneEvent(NXNode visual)
            : base(visual, Name, true, false)
        {

        }

    }

}
