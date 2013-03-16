namespace XEngine.Forms
{

    public class XFormsResetEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-reset";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsResetEvent()
            : base(Name, true, false)
        {

        }

    }

}
