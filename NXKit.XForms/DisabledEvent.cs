namespace NXKit.XForms
{

    public class DisabledEvent :
        XFormsEvent
    {

        public static readonly string Name = "xforms-disabled";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public DisabledEvent(NXNode visual)
            : base(visual, Name, true, false)
        {

        }

    }

}
