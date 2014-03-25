namespace NXKit.XForms
{

    public class InsertEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-insert";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public InsertEvent(NXNode visual)
            : base(visual, Name, true, false)
        {

        }

    }

}
