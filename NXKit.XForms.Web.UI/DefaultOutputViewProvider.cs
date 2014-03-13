using System.ComponentModel.Composition;

namespace NXKit.XForms.Web.UI
{

    /// <summary>
    /// Exports the output view controls in the XForms assembly.
    /// </summary>
    [Export(typeof(IOutputViewProvider))]
    public class DefaultOutputViewProvider :
        AssemblyOutputViewProvider
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public DefaultOutputViewProvider()
            : base(typeof(DefaultOutputViewProvider).Assembly)
        {

        }

    }

}
