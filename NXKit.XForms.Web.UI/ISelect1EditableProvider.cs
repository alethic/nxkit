using System.Web.UI;

using NXKit.Web.UI;

namespace NXKit.XForms.Web.UI
{

    public interface ISelect1EditableProvider
    {

        /// <summary>
        /// Creates a new <see cref="Input"/> for the given <see cref="XFormsSelect1Visual"/>.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="visual"></param>
        /// <returns></returns>
        Control Create(View view, XFormsSelect1Visual visual);

    }

}
