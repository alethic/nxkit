namespace NXKit.XForms
{

    public class XFormsValueChangedEvent :
        XFormsEvent
    {

        public static readonly string Name = "xforms-value-changed";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsValueChangedEvent(NXNode visual)
            : base(visual, Name, true, false)
        {

        }

    }

}
