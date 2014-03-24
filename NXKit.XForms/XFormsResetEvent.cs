namespace NXKit.XForms
{

    public class XFormsResetEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-reset";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsResetEvent(NXNode visual)
            : base(visual, Name, true, false)
        {

        }

    }

}
