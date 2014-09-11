using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
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
        /// Resolves the <see cref="ExportProvider"/> for the given <see cref="XDocument"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static ExportProvider Exports(this XDocument self)
        {
            Contract.Requires<ArgumentNullException>(self != null);
            Contract.Ensures(Contract.Result<ExportProvider>() != null);

            return self.Annotation<ExportProvider>();
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

        /// <summary>
        /// Clones the specified <see cref="XDocument"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static XDocument Clone(this XDocument self)
        {
            Contract.Requires<ArgumentNullException>(self != null);

            return XCloneTransformer.Default.Visit(self);
        }

    }

}
