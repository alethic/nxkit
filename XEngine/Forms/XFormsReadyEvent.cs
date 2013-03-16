namespace XEngine.Forms
{

    public class XFormsReadyEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-ready";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsReadyEvent()
            : base(Name, true, false)
        {

        }

    }

}
