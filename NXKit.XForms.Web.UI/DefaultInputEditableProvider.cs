using System.ComponentModel.Composition;

namespace NXKit.XForms.Web.UI
{

    /// <summary>
    /// Exports the editable controls in the XForms assembly.
    /// </summary>
    [Export(typeof(IInputEditableProvider))]
    public class DefaultInputEditableProvider :
        AssemblyInputEditableProvider
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public DefaultInputEditableProvider()
            : base(typeof(DefaultInputEditableProvider).Assembly)
        {

        }

    }

}
