using System.Web.UI;

using NXKit.Web.UI;

namespace NXKit.XForms.Web.UI
{

    /// <summary>
    /// Resolves XSD types to <see cref="Input"/> instances.
    /// </summary>
    public interface IInputEditableProvider
    {

        /// <summary>
        /// Creates a new <see cref="Input"/> for the given <see cref="XFormsInputVisual"/>.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        /// <returns></returns>
        Control Create(View view, XFormsInputVisual visual);

    }

}
