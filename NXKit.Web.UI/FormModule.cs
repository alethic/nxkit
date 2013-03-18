namespace NXKit.Web.UI
{

    public abstract class FormModule
    {

        public const string ViewParameter = "View";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        public FormModule(View view)
        {
            View = view;
        }

        public View View { get; private set; }

    }

}
