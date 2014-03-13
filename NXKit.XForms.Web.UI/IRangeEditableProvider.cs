using System.Web.UI;

using NXKit.Web.UI;

namespace NXKit.XForms.Web.UI
{

    /// <summary>
    /// Resolves XSD types to range controls.
    /// </summary>
    public interface IRangeEditableProvider
    {

        /// <summary>
        /// Creates a new <see cref="Input"/> for the given <see cref="XFormsRangeVisual"/>.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        /// <returns></returns>
        Control Create(View view, XFormsRangeVisual visual);

    }

}
