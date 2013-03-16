namespace ISIS.Forms.XForms
{

    public class XFormsRevalidateEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-revalidate";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsRevalidateEvent()
            : base(Name, true, true)
        {

        }

    }

}
