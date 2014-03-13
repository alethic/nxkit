using System.ComponentModel.Composition;

namespace NXKit.XForms.Web.UI
{

    /// <summary>
    /// Exports the editable controls in the XForms assembly.
    /// </summary>
    [Export(typeof(IRangeEditableProvider))]
    public class DefaultRangeEditableProvider :
        AssemblyRangeEditableProvider
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public DefaultRangeEditableProvider()
            : base(typeof(DefaultRangeEditableProvider).Assembly)
        {

        }

    }

}
