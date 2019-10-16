using System;
using System.Collections.Generic;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.Xml
{

    /// <summary>
    /// Provides extension methods for <see cref="XDocument"/> instances.
    /// </summary>
    public static class XDocumentExtensions
    {

        /// <summary>
        /// Resolves the <see cref="ExportProvider"/> for the given <see cref="XDocument"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static ICompositionContext Exports(this XDocument self)
        {
            if (self == null)
                throw new ArgumentNullException(nameof(self));

            return self.Annotation<ICompositionContext>();
        }

        /// <summary>
        /// Resolves a <see cref="XElement"/> by IDREF from the vantage point of this <see cref="XDocument"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static XElement ResolveId(this XDocument self, string id)
        {
            if (self == null)
                throw new ArgumentNullException(nameof(self));
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            return self.Root.ResolveId(id);
        }

        /// <summary>
        /// Gets the namespace associated with a prefix for this <see cref="XDocument"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static XNamespace GetNamespaceOfPrefix(this XDocument self, string prefix)
        {
            if (self == null)
                throw new ArgumentNullException(nameof(self));
            if (prefix == null)
                throw new ArgumentNullException(nameof(prefix));

            return prefix != "" ? self.Root.GetNamespaceOfPrefix(prefix) : self.Root.GetDefaultNamespace();
        }

        /// <summary>
        /// Gets the prefix associated with a namespace for this <see cref="XDocument"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="ns"></param>
        /// <returns></returns>
        public static string GetPrefixOfNamespace(this XDocument self, XNamespace ns)
        {
            if (self == null)
                throw new ArgumentNullException(nameof(self));
            if (ns == null)
                throw new ArgumentNullException(nameof(ns));

            return self.Root.GetPrefixOfNamespace(ns);
        }

        /// <summary>
        /// Returns a collection of descendant nodes for this document, including the document itself.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static IEnumerable<XNode> DescendantsAndSelf(this XDocument document)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            yield return document;
            foreach (var node in document.DescendantNodes())
                yield return node;
        }

        /// <summary>
        /// Clones the specified <see cref="XDocument"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static XDocument Clone(this XDocument self)
        {
            if (self == null)
                throw new ArgumentNullException(nameof(self));

            return XCloneTransformer.Default.Visit(self);
        }

    }

}
