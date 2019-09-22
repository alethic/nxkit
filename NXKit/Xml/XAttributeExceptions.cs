using System;
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
            if (self == null)
                throw new ArgumentNullException(nameof(self));
            if (self.Parent == null)
                throw new ArgumentNullException(nameof(self));
            if (id == null)
                throw new ArgumentNullException(nameof(id));

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
            if (self == null)
                throw new ArgumentNullException(nameof(self));
            if (self.Parent == null)
                throw new ArgumentNullException(nameof(self));
            if (prefix == null)
                throw new ArgumentNullException(nameof(prefix));

            // resolve through element
            if (prefix == "")
                return self.Parent.GetDefaultNamespace();
            else
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
            if (self == null)
                throw new ArgumentNullException(nameof(self));
            if (self.Parent == null)
                throw new ArgumentNullException(nameof(self));
            if (ns == null)
                throw new ArgumentNullException(nameof(ns));

            return self.Parent.GetPrefixOfNamespace(ns);
        }

        #endregion

    }

}
