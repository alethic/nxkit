namespace NXKit.XForms
{

    public class BindingExceptionEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-binding-exception";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public BindingExceptionEvent(NXNode visual)
            : base(visual, Name, true, false)
        {

        }

    }

}
