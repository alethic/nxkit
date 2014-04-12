using System;
using System.Diagnostics.Contracts;
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
            Contract.Requires<ArgumentNullException>(self != null);
            Contract.Requires<ArgumentNullException>(self.Parent != null);
            Contract.Requires<ArgumentNullException>(id != null);

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
            Contract.Requires<ArgumentNullException>(self != null);
            Contract.Requires<ArgumentNullException>(self.Parent != null);
            Contract.Requires<ArgumentNullException>(prefix != null);

            var element = self as XElement;
            if (element != null)
                return element.GetNamespaceOfPrefix(prefix);

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
            Contract.Requires<ArgumentNullException>(self != null);
            Contract.Requires<ArgumentNullException>(self is XAttribute || self is XNode);
            Contract.Requires<ArgumentException>(self.Parent != null);
            Contract.Requires<ArgumentNullException>(ns != null);

            var element = self as XElement;
            if (element != null)
                return element.GetPrefixOfNamespace(ns);

            var document = self as XDocument;
            if (document != null)
                return document.GetPrefixOfNamespace(ns);

            throw new InvalidOperationException();
        }

    }

}
