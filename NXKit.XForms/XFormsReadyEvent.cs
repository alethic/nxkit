namespace NXKit.XForms
{

    public class XFormsReadyEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-ready";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsReadyEvent(NXNode visual)
            : base(visual, Name, true, false)
        {

        }

    }

}
