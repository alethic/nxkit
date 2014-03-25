namespace NXKit.XForms
{

    public class DeleteEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-delete";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public DeleteEvent(NXNode visual)
            : base(visual, Name, true, false)
        {

        }

    }

}
