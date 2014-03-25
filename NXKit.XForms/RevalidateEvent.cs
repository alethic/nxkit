namespace NXKit.XForms
{

    public class RevalidateEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-revalidate";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public RevalidateEvent(NXNode visual)
            : base(visual, Name, true, true)
        {

        }

    }

}
