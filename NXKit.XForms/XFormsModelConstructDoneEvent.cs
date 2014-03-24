namespace NXKit.XForms
{

    public class XFormsModelConstructDoneEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-model-construct-done";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsModelConstructDoneEvent(NXNode visual)
            : base(visual, Name, true, false)
        {

        }

    }

}
