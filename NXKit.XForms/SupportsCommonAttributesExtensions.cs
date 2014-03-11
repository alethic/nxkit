using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// The Common Attribute Collection applies to every element in the XForms namespace.
    /// </summary>
    public static class SupportsCommonAttributesExtensions
    {

        /// <summary>
        /// Foreign attributes are allowed on all XForms elements.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static XAttribute AnyAttribute<T>(this T self, XName name)
            where T : StructuralVisual, ISupportsCommonAttributes
        {
            return self.Element.Attribute(name);
        }

    }

}
