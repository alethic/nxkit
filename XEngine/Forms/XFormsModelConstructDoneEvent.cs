namespace XEngine.Forms
{

    public class XFormsModelConstructDoneEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-model-construct-done";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsModelConstructDoneEvent()
            : base(Name, true, false)
        {

        }

    }

}
