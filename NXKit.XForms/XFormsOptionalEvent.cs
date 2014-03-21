namespace NXKit.XForms
{

    public class XFormsOptionalEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-optional";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsOptionalEvent()
            : base(Name, true, false)
        {

        }

    }

}
