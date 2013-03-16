namespace XEngine.Forms.XForms
{

    public class XFormsRebuildEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-rebuild";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsRebuildEvent()
            : base(Name, true, true)
        {

        }

    }

}
