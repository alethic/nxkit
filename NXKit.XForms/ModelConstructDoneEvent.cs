namespace NXKit.XForms
{

    public class ModelConstructDoneEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-model-construct-done";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ModelConstructDoneEvent(NXNode visual)
            : base(visual, Name, true, false)
        {

        }

    }

}
