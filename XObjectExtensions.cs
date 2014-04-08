using System.Linq;
using System.Xml.Linq;

using NXKit.Util;

namespace NXKit
{

    /// <summary>
    /// Provides various extension methods for working with <see cref="XObject"/> instances.
    /// </summary>
    public static class XObjectExtensions
    {

        /// <summary>
        /// Resolves the <see cref="NXDocumentHost"/> for the given <see cref="XObject"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static NXDocumentHost Host(this XObject self)
        {
            return self.AncestorsAndSelf()
                .SelectMany(i => i.Annotations<NXDocumentHost>())
                .FirstOrDefault();
        }

    }

}
