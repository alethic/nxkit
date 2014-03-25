namespace NXKit.XForms
{

    public class LinkExceptionEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-link-exception";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public LinkExceptionEvent(NXNode visual)
            : base(visual, Name, true, false)
        {

        }

    }

}
