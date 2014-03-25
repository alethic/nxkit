namespace NXKit.XForms
{

    public class ModelConstructEvent : XFormsEvent
    {

        public static readonly string Name = "xforms-model-construct";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ModelConstructEvent(NXNode visual)
            : base(visual, Name, true, false)
        {

        }

    }

}
