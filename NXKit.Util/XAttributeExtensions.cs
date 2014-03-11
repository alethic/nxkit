using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.Util
{

    /// <summary>
    /// Provides extension methods for working with <see cref="XAttribute"/> instances.
    /// </summary>
    public static class XAttributeExtensions
    {

        /// <summary>
        /// Obtains the default namespace to be applied to the given attribute value.
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        static XNamespace GetDefaultNamespace(XAttribute attribute)
        {
            return attribute.Name.Namespace.NamespaceName != "" ? attribute.Name.Namespace : attribute.Parent.Name.Namespace;
        }

        /// <summary>
        /// Obtains the fully qualified version of the <see cref="XAttribute"/>'s name.
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static XName FullyQualifiedName(this XAttribute attribute)
        {
            return GetDefaultNamespace(attribute) + attribute.Name.LocalName;
        }

        /// <summary>
        /// Obtains the value of the specified node as an <see cref="XName"/> given it's current location in the document hierarchy.
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static XName ValueAsXName(this XAttribute attribute)
        {
            Contract.Requires<ArgumentNullException>(attribute != null);

            var value = attribute.Value;
            if (value.TrimToNull() == null)
                return null;

            var a = value.Split(':');
            if (a.Length > 2)
                throw new FormatException("Attribute value is not a valid QName.");

            var prefix = a.Length == 2 ? a[0].TrimToNull() : null;
            var localName = a.Length == 2 ? a[1].TrimToNull() : a[0].TrimToNull();

            if (localName == null)
                throw new FormatException("Attribute value has no local name.");

            // determine namespace
            var ns = prefix != null ? 
                attribute.Parent.GetNamespaceOfPrefix(prefix) : 
                GetDefaultNamespace(attribute);

            return ns + localName;
        }

    }

}
