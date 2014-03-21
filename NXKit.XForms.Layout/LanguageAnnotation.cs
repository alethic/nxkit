namespace NXKit.XForms.Layout
{

    public class LanguageAnnotation : VisualAnnotation
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="code"></param>
        public LanguageAnnotation(string code)
            : base()
        {
            Code = code.ToLowerInvariant();
        }

        public string Code { get; set; }

    }

}
