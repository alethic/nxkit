namespace NXKit.XForms
{

    public class XFormsDisabledEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-disabled";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsDisabledEvent()
            : base(Name, true, false)
        {

        }

    }

}
