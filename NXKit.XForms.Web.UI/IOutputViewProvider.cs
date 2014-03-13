using System.Web.UI;

using NXKit.Web.UI;

namespace NXKit.XForms.Web.UI
{

    /// <summary>
    /// Resolves views for rendering output visuals.
    /// </summary>
    public interface IOutputViewProvider
    {

        /// <summary>
        /// Creates a new <see cref="Control"/> for the given <see cref="Visual"/>.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        /// <returns></returns>
        Control Create(View view, XFormsOutputVisual visual);

    }

}
