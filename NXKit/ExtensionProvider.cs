using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit
{

    /// <summary>
    /// Queries for <see cref="IExtension"/> implementors.
    /// </summary>
    [Export(typeof(ExtensionProvider))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class ExtensionProvider
    {

        static readonly ConcurrentDictionary<Type, IExtensionPredicate> predicates;

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static ExtensionProvider()
        {
            predicates = new ConcurrentDictionary<Type, IExtensionPredicate>();
        }

        /// <summary>
        /// Tests a given extension metadata set against the given object.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        internal static bool Predicate(XObject obj, IExtensionMetadata metadata)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            if (metadata == null)
                throw new ArgumentNullException(nameof(metadata));

            if (!metadata.ObjectType.HasFlag(GetObjectType(obj)))
                return false;

            if (obj is XElement)
                if (!IsMatch((XElement)obj, metadata.NamespaceName, metadata.LocalName))
                    return false;

            if (obj is XAttribute)
                if (!IsMatch((XAttribute)obj, metadata.NamespaceName, metadata.LocalName))
                    return false;

            // test against specified predicate type
            var predicate = metadata.PredicateType != null ? predicates.GetOrAdd(metadata.PredicateType, _ => (IExtensionPredicate)Activator.CreateInstance(_)) : null;
            if (predicate != null)
                if (!predicate.IsMatch(obj))
                    return false;

            return true;
        }

        /// <summary>
        /// Gets the <see cref="ExtensionObjectType"/> for a given <see cref="XObject"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal static ExtensionObjectType GetObjectType(XObject obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

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

        internal static bool IsMatch(XElement element, string namespaceName, string localName)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            if (namespaceName != null &&
                namespaceName != element.Name.NamespaceName)
                return false;

            if (localName != null &&
                localName != element.Name.LocalName)
                return false;

            return true;
        }

        internal static bool IsMatch(XAttribute attribute, string namespaceName, string localName)
        {
            if (attribute == null)
                throw new ArgumentNullException(nameof(attribute));

            if (namespaceName != null &&
                namespaceName != attribute.Name.NamespaceName)
                return false;

            if (localName != null &&
                localName != attribute.Name.LocalName)
                return false;

            return true;
        }


        readonly XObject obj;
        readonly IEnumerable<Lazy<IExtension>> extensions;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="extensions"></param>
        [ImportingConstructor]
        public ExtensionProvider(
            XObject obj,
            [ImportMany] IEnumerable<Lazy<IExtension, IDictionary<string, object>>> extensions)
        {
            this.obj = obj ?? throw new ArgumentNullException(nameof(obj));
            this.extensions = GetExtensions(extensions?? throw new ArgumentNullException(nameof(extensions))).ToList();
        }

        /// <summary>
        /// Gets the set of all available extensions for this node.
        /// </summary>
        public IEnumerable<Lazy<IExtension>> Extensions
        {
            get { return extensions; }
        }

        /// <summary>
        /// Returns each <see cref="IExtension"/> implementation supported on this object.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Lazy<IExtension>> GetExtensions(IEnumerable<Lazy<IExtension, IDictionary<string, object>>> extensions)
        {
            if (extensions == null)
                throw new ArgumentNullException(nameof(extensions));

            foreach (var extension in extensions)
                foreach (var metadata in ExtensionMetadata.Extract(extension.Metadata))
                    if (Predicate(obj, metadata))
                        yield return extension;
        }

    }

    /// <summary>
    /// Provides interface lookup using the container.
    /// </summary>
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class ExtensionProvider<T>
        where T : class
    {

        readonly XObject obj;
        readonly ExtensionProvider provider;
        readonly IEnumerable<Lazy<T, IDictionary<string, object>>> extensions;
        readonly Lazy<IEnumerable<T>> query;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="provider"></param>
        [ImportingConstructor]
        public ExtensionProvider(
            XObject obj,
            ExtensionProvider provider,
            [ImportMany] IEnumerable<Lazy<T, IDictionary<string, object>>> extensions)
        {

            this.obj = obj ?? throw new ArgumentNullException(nameof(obj));
            this.provider = provider ?? throw new ArgumentNullException(nameof(provider));
            this.extensions = extensions ?? throw new ArgumentNullException(nameof(extensions));
            this.query = new Lazy<IEnumerable<T>>(() => GetExtensions());
        }

        /// <summary>
        /// Gets the set of extensions implementing the given interface.
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetExtensions()
        {
            return provider.Extensions
                .Select(i => i.Value)
                .OfType<T>()
                .Concat(extensions
                    .Where(i => ExtensionMetadata.Extract(i.Metadata).Any(j => ExtensionProvider.Predicate(obj, j)))
                    .Select(i => i.Value))
                .Distinct()
                .ToList();
        }

        /// <summary>
        /// Exports typed extensions.
        /// </summary>
        [Export(typeof(Extension<>))]
        public Extension<T> Extension
        {
            get { return new Extension<T>(() => query.Value.FirstOrDefault()); }
        }

        /// <summary>
        /// Exports typed extensions.
        /// </summary>
        [Export(typeof(ExtensionQuery<>))]
        public ExtensionQuery<T> ExtensionQuery
        {
            get { return new ExtensionQuery<T>(() => query.Value); }
        }

    }

}
