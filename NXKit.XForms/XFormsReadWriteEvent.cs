namespace NXKit.XForms
{

    public class XFormsReadWriteEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-readwrite";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsReadWriteEvent()
            : base(Name, true, false)
        {

        }

    }

}
