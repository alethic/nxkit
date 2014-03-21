namespace NXKit.XForms
{

    public class XFormsSubmitEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-submit";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsSubmitEvent()
            : base(Name, true, true)
        {

        }

    }
}
