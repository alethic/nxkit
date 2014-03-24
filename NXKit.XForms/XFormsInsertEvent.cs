namespace NXKit.XForms
{

    public class XFormsInsertEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-insert";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsInsertEvent(NXNode visual)
            : base(visual, Name, true, false)
        {

        }

    }

}
