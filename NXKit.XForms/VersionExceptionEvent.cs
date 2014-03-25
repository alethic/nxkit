namespace NXKit.XForms
{

    public class VersionExceptionEvent :
        XFormsEvent
    {

        public static readonly string Name = "xforms-version-exception";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public VersionExceptionEvent(NXNode visual)
            : base(visual, Name, true, false)
        {

        }

    }

}
