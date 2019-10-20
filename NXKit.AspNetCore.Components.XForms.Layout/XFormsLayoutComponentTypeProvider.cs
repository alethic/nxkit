namespace NXKit.AspNetCore.Components.XForms.Layout
{

    [Extension(typeof(INXComponentTypeProvider))]
    public class XFormsLayoutComponentTypeProvider : AssemblyComponentTypeProviderBase
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public XFormsLayoutComponentTypeProvider() :
            base(typeof(XFormsLayoutComponentTypeProvider).Assembly)
        {

        }

    }

}
