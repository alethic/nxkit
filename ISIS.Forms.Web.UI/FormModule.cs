namespace ISIS.Forms.Web.UI
{

    public abstract class FormModule
    {

        public const string ViewParameter = "View";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        public FormModule(FormView view)
        {
            View = view;
        }

        public FormView View { get; private set; }

    }

}
