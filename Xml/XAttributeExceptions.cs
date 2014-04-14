using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.Xml
{

    /// <summary>
    /// Provides extension methods for <see cref="XAttribute"/> instances.
    /// </summary>
    public static class XAttributeExtensions
    {

        /// <summary>
        /// Resolves a <see cref="XElement"/> by IDREF from the vantage point of this <see cref="XElement"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static XElement ResolveId(this XAttribute self, string id)
        {
            Contract.Requires<ArgumentNullException>(self != null);
            Contract.Requires<ArgumentNullException>(self.Parent != null);

            return self.Parent.ResolveId(id);
        }

        #region Naming

        /// <summary>
        /// Gets the namespace associated with a prefix for this <see cref="XAttribute"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static XNamespace GetNamespaceOfPrefix(this XAttribute self, string prefix)
        {
            Contract.Requires<ArgumentNullException>(self != null);
            Contract.Requires<ArgumentException>(self.Parent != null);
            Contract.Requires<ArgumentNullException>(prefix != null);

            if (prefix == "")
                // default prefix
                if (self.Name.Namespace != XNamespace.None)
                    // attribute's specified prefix takes priority
                    return self.Name.Namespace;
                else
                    // attribute's element provides default
                    return self.Parent.Name.Namespace;
            else
                // resolve through element
                return self.Parent.GetNamespaceOfPrefix(prefix);
        }

        /// <summary>
        /// Gets the prefix associated with a namespace for this <see cref="XAttribute"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="ns"></param>
        /// <returns></returns>
        public static string GetPrefixOfNamespace(this XAttribute self, XNamespace ns)
        {
            Contract.Requires<ArgumentNullException>(self != null);
            Contract.Requires<ArgumentException>(self.Parent != null);
            Contract.Requires<ArgumentNullException>(ns != null);

            return self.Parent.GetPrefixOfNamespace(ns);
        }

        #endregion

    }

}
