using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit
{

    /// <summary>
    /// Provides interface lookup using the container.
    /// </summary>
    [Export(typeof(IExtensionProvider))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class DefaultExtensionProvider :
        IExtensionProvider
    {

        static Func<T> GetLazyFunc<T>(Lazy<T> inner)
        {
            return () => inner.Value;
        }

        readonly XObject obj;
        readonly IEnumerable<Lazy<IExtension, IExtensionMetadata>> extensions;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="predicates"></param>
        [ImportingConstructor]
        public DefaultExtensionProvider(
            XObject obj,
            [ImportMany] IEnumerable<Lazy<IExtension, IExtensionMetadata>> extensions)
        {
            Contract.Requires<ArgumentNullException>(obj != null);
            Contract.Requires<ArgumentNullException>(extensions != null);

            this.obj = obj;
            this.extensions = extensions;
        }

        public IEnumerable<T> GetExtensions<T>(XObject obj)
        {
            return GetExtensions(obj, typeof(T)).OfType<T>();
        }

        public IEnumerable<object> GetExtensions(XObject obj, Type type)
        {
            if (obj != this.obj)
                yield break;

            foreach (var extension in extensions)
                if (Predicate(obj, extension.Metadata, type))
                    yield return extension.Value;
        }

        /// <summary>
        /// Tests a given extension metadata set against the given object.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        bool Predicate(XObject obj, IExtensionMetadata metadata, Type interfaceType)
        {
            if (metadata.ObjectType != GetObjectType(obj))
                return false;

            if (obj is XElement)
                return IsMatch((XElement)obj, metadata.NamespaceName, metadata.LocalName);

            if (obj is XAttribute)
                return IsMatch((XAttribute)obj, metadata.NamespaceName, metadata.LocalName);

            // test against specified predicate type
            var predicate = metadata.PredicateType != null ? (IExtensionPredicate)Activator.CreateInstance(metadata.PredicateType) : null;
            if (predicate != null)
                if (!predicate.IsMatch(obj, null))
                    return false;

            return true;
        }

        /// <summary>
        /// Gets the <see cref="ExtensionObjectType"/> for a given <see cref="XObject"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        ExtensionObjectType GetObjectType(XObject obj)
        {
            if (obj is XDocument)
                return ExtensionObjectType.Document;
            if (obj is XElement)
                return ExtensionObjectType.Element;
            if (obj is XText)
                return ExtensionObjectType.Text;
            if (obj is XAttribute)
                return ExtensionObjectType.Attribute;

            throw new NotSupportedException();
        }

        bool IsMatch(XElement element, string namespaceName, string localName)
        {
            if (namespaceName != null &&
                namespaceName != element.Name.NamespaceName)
                return false;

            if (localName != null &&
                localName != element.Name.LocalName)
                return false;

            return true;
        }

        bool IsMatch(XAttribute element, string namespaceName, string localName)
        {
            if (namespaceName != null &&
                namespaceName != element.Name.NamespaceName)
                return false;

            if (localName != null &&
                localName != element.Name.LocalName)
                return false;

            return true;
        }

    }

}
