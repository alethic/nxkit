namespace NXKit.XForms
{

    public class ValueChangedEvent :
        XFormsEvent
    {

        public static readonly string Name = "xforms-value-changed";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ValueChangedEvent(NXNode visual)
            : base(visual, Name, true, false)
        {

        }

    }

}
