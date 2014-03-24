namespace NXKit.XForms
{

    public class XFormsRebuildEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-rebuild";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsRebuildEvent(NXNode visual)
            : base(visual, Name, true, true)
        {

        }

    }

}
