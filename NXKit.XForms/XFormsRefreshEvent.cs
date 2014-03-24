namespace NXKit.XForms
{

    public class XFormsRefreshEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-refresh";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsRefreshEvent(NXNode visual)
            : base(visual,Name, true, true)
        {

        }

    }

}
