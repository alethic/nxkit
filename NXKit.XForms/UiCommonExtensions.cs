using System.Xml.Linq;

using NXKit.Util;

namespace NXKit.XForms
{

    public static class UiCommonExtensions
    {

        /// <summary>
        /// Author-optional attribute to define an appearance hint. If absent, the user agent may freely choose any suitable rendering.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="visual"></param>
        /// <returns></returns>
        public static XName GetAppearance<T>(this T visual)
            where T : StructuralVisual, IUiCommon
        {
            var a = visual.Document.GetModule<XFormsModule>().ResolveAttribute(visual.Element, "appearance");
            return a != null ? a.ValueAsXName() : null;
        }

    }

}
