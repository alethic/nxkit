namespace NXKit.XForms
{

    public class XFormsRefreshEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-refresh";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsRefreshEvent()
            : base(Name, true, true)
        {

        }

    }

}
