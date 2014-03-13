using System.ComponentModel.Composition;

namespace NXKit.XForms.Web.UI
{

    /// <summary>
    /// Exports the editable controls in the XForms assembly.
    /// </summary>
    [Export(typeof(ISelect1EditableProvider))]
    public class DefaultSelect1EditableProvider :
        AssemblySelect1EditableProvider
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public DefaultSelect1EditableProvider()
            : base(typeof(DefaultSelect1EditableProvider).Assembly)
        {

        }

    }

}
