namespace NXKit.XForms
{

    public class XFormsSubmitErrorEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-submit-error";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsSubmitErrorEvent(NXNode visual)
            : base(visual, Name, true, false)
        {

        }

    }

}
