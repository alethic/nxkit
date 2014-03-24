namespace NXKit.XForms
{

    public class XFormsReadOnlyEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-readonly";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsReadOnlyEvent(NXNode visual)
            : base(visual, Name, true, false)
        {

        }

    }

}
