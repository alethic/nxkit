namespace NXKit.AspNetCore.Components.XForms
{

    [Extension(typeof(INXComponentTypeProvider))]
    public class XFormsComponentTypeProvider : AssemblyComponentTypeProviderBase
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsComponentTypeProvider() :
            base(typeof(XFormsComponentTypeProvider).Assembly)
        {

        }

    }

}
