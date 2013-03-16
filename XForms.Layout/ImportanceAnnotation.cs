namespace ISIS.Forms.Layout
{

    public class ImportanceAnnotation : VisualAnnotation
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="code"></param>
        public ImportanceAnnotation(Importance importance)
            : base()
        {
            Importance = importance;
        }

        public Importance Importance { get; set; }

    }

}
