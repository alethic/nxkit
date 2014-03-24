namespace NXKit.XForms
{

    public class XFormsEnabledEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-enabled";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsEnabledEvent(NXNode visual)
            : base(visual, Name, true, false)
        {

        }

    }

}
