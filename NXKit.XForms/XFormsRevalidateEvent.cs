namespace NXKit.XForms
{

    public class XFormsRevalidateEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-revalidate";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsRevalidateEvent(NXNode visual)
            : base(visual, Name, true, true)
        {

        }

    }

}
