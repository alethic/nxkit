namespace NXKit.XForms
{

    public class RebuildEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-rebuild";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public RebuildEvent(NXNode visual)
            : base(visual, Name, true, true)
        {

        }

    }

}
