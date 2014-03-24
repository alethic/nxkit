namespace NXKit.XForms
{

    public class XFormsDeleteEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-delete";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsDeleteEvent(NXNode visual)
            : base(visual, Name, true, false)
        {

        }

    }

}
