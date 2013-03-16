namespace XEngine.Forms
{

    public class XFormsReadOnlyEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-readonly";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsReadOnlyEvent()
            : base(Name, true, false)
        {

        }

    }

}
