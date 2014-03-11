using System.Xml.Linq;

using NXKit.Util;

namespace NXKit.XForms
{

    public static class SupportsIncrementalAttributeExtensions
    {

        public static bool Incremental<T>(this T self)
            where T : StructuralVisual, ISupportsIncrementalAttribute
        {
            var a = self.Document.GetModule<XFormsModule>().ResolveAttribute(self.Element, "incremental");
            return a != null ? a.Value == "true" : false;
        }

    }

}
