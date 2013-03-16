namespace XEngine.Forms.XForms
{

    public class XFormsInsertEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-insert";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsInsertEvent()
            : base(Name, true, false)
        {

        }

    }

}
