namespace NXKit.XForms
{

    public class RefreshEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-refresh";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public RefreshEvent(NXNode visual)
            : base(visual,Name, true, true)
        {

        }

    }

}
