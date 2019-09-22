using System;
using System.Xml.Linq;

namespace NXKit.Xml
{

    /// <summary>
    /// Provides extension methods for <see cref="XNode"/> instances.
    /// </summary>
    public static class XNodeExtensions
    {

        /// <summary>
        /// Resolves a <see cref="XElement"/> by IDREF from the vantage point of this <see cref="XNode"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static XElement ResolveId(this XNode self, string id)
        {
            if (self == null)
                throw new ArgumentNullException(nameof(self));
            if (self.Parent == null)
                throw new ArgumentNullException(nameof(self));
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var element = self as XElement;
            if (element != null)
                return element.ResolveId(id);

            var document = self as XDocument;
            if (document != null)
                return document.ResolveId(id);

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets the namespace associated with a prefix for this <see cref="XNode"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static XNamespace GetNamespaceOfPrefix(this XNode self, string prefix)
        {
            if (self == null)
                throw new ArgumentNullException(nameof(self));
            if (self.Parent == null)
                throw new ArgumentNullException(nameof(self));
            if (prefix == null)
                throw new ArgumentNullException(nameof(prefix));

            var element = self as XElement;
            if (element != null)
                return prefix != "" ? element.GetNamespaceOfPrefix(prefix) : element.GetDefaultNamespace();

            var document = self as XDocument;
            if (document != null)
                return document.GetNamespaceOfPrefix(prefix);

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets the prefix associated with a namespace for this <see cref="XNode"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="ns"></param>
        /// <returns></returns>
        public static string GetPrefixOfNamespace(this XNode self, XNamespace ns)
        {
            if (self == null)
                throw new ArgumentNullException(nameof(self));
            if (self.Parent == null)
                throw new ArgumentNullException(nameof(self));
            if (ns == null)
                throw new ArgumentNullException(nameof(ns));

            var element = self as XElement;
            if (element != null)
                return element.GetPrefixOfNamespace(ns);

            var document = self as XDocument;
            if (document != null)
                return document.GetPrefixOfNamespace(ns);

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Clones the specified <see cref="XNode"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static XNode Clone(this XNode self)
        {
            if (self == null)
                throw new ArgumentNullException(nameof(self));

            return XCloneTransformer.Default.Visit(self);
        }

    }

}
