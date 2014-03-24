namespace NXKit.XForms
{

    public class XFormsReadWriteEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-readwrite";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsReadWriteEvent(NXNode visual)
            : base(visual, Name, true, false)
        {

        }

    }

}
