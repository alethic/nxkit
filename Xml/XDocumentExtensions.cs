using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.Xml
{

    /// <summary>
    /// Provides extension methods for <see cref="XDocument"/> instances.
    /// </summary>
    public static class XDocumentExtensions
    {

        /// <summary>
        /// Resolves the <see cref="NXDocumentHost"/> for the given <see cref="XDocument"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static NXDocumentHost Host(this XDocument self)
        {
            Contract.Requires<ArgumentNullException>(self != null);
            Contract.Ensures(Contract.Result<NXDocumentHost>() != null);

            return self.Annotation<NXDocumentHost>();
        }

        /// <summary>
        /// Resolves a <see cref="XElement"/> by IDREF from the vantage point of this <see cref="XDocument"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static XElement ResolveId(this XDocument self, string id)
        {
            Contract.Requires<ArgumentNullException>(self != null);
            Contract.Requires<ArgumentNullException>(id != null);

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
            Contract.Requires<ArgumentNullException>(self != null);
            Contract.Requires<ArgumentNullException>(prefix != null);

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
            Contract.Requires<ArgumentNullException>(self != null);
            Contract.Requires<ArgumentNullException>(ns != null);

            return self.Root.GetPrefixOfNamespace(ns);
        }

        /// <summary>
        /// Returns a collection of descendant nodes for this document, including the document itself.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static IEnumerable<XNode> DescendantsAndSelf(this XDocument document)
        {
            Contract.Requires<ArgumentNullException>(document != null);

            yield return document;
            foreach (var node in document.DescendantNodes())
                yield return node;
        }

    }

}
