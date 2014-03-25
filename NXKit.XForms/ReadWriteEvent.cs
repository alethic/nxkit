namespace NXKit.XForms
{

    public class ReadWriteEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-readwrite";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ReadWriteEvent(NXNode visual)
            : base(visual, Name, true, false)
        {

        }

    }

}
