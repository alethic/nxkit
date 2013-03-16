namespace XEngine.Forms
{

    public class XFormsDeleteEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-delete";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsDeleteEvent()
            : base(Name, true, false)
        {

        }

    }

}
