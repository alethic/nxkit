using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using NXKit.Composition;
using NXKit.Util;

namespace NXKit
{

    [Export(typeof(IExtensionProvider))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class ContainerExtensionProvider :
        IExtensionProvider
    {

        /// <summary>
        /// Implements <see cref="IExtensionMetadata"/>.
        /// </summary>
        class Metadata :
            IExtensionMetadata
        {

            /// <summary>
            /// Creates a <see cref="Metadata"/> instance from the given metadata dictionary.
            /// </summary>
            /// <param name="metadata"></param>
            /// <returns></returns>
            public static IEnumerable<IExtensionMetadata> Extract(IDictionary<string, object> metadata)
            {
                var objectTypes = (ExtensionObjectType[])metadata.GetOrDefault("ObjectType");
                var localNames = (string[])metadata.GetOrDefault("LocalName");
                var namespaceNames = (string[])metadata.GetOrDefault("NamespaceName");
                var predicateTypes = (Type[])metadata.GetOrDefault("PredicateType");
                var interfaceTypes = (Type[])metadata.GetOrDefault("InterfaceType");

                // find shortest array
                int length = objectTypes.Length;
                length = Math.Min(length, localNames.Length);
                length = Math.Min(length, namespaceNames.Length);
                length = Math.Min(length, predicateTypes.Length);
                length = Math.Min(length, interfaceTypes.Length);

                // extract metadata
                for (int i = 0; i < length; i++)
                    yield return new Metadata(objectTypes[i], localNames[i], namespaceNames[i], predicateTypes[i], interfaceTypes[i]);
            }

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="objectType"></param>
            /// <param name="localName"></param>
            /// <param name="namespaceName"></param>
            /// <param name="predicateType"></param>
            /// <param name="interfaceType"></param>
            public Metadata(ExtensionObjectType objectType, string localName, string namespaceName, Type predicateType, Type interfaceType)
            {
                ObjectType = objectType;
                LocalName = localName;
                NamespaceName = namespaceName;
                PredicateType = predicateType;
                InterfaceType = interfaceType;
            }

            public ExtensionObjectType ObjectType { get; set; }

            public string LocalName { get; set; }

            public string NamespaceName { get; set; }

            public Type PredicateType { get; set; }

            public Type InterfaceType { get; set; }

        }

        static Func<T> GetLazyFunc<T>(Lazy<T> inner)
        {
            return () => inner.Value;
        }

        readonly XObject obj;
        readonly IEnumerable<Tuple<Func<IExtension>, IEnumerable<IExtensionMetadata>>> extensions;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="predicates"></param>
        [ImportingConstructor]
        public ContainerExtensionProvider(
            XObject obj,
            [ImportMany] IEnumerable<Lazy<IExtension, IDictionary<string, object>>> extensions)
        {
            Contract.Requires<ArgumentNullException>(obj != null);
            Contract.Requires<ArgumentNullException>(extensions != null);

            this.obj = obj;
            this.extensions = extensions
                .Select(i => Tuple.Create(
                    GetLazyFunc(i),
                    Metadata.Extract(i.Metadata)))
                .ToList()
                .AsEnumerable();
        }

        public IEnumerable<T> GetExtensions<T>(XObject obj)
        {
            return GetExtensionsInternal(obj, typeof(T)).OfType<T>();
        }

        public IEnumerable<object> GetExtensions(XObject obj, Type type)
        {
            return GetExtensionsInternal(obj, type);
        }

        IEnumerable<object> GetExtensionsInternal(XObject obj, Type type)
        {
            foreach (var extension in extensions)
                if (extension.Item2.Any(j => Predicate(obj, j, type)))
                    yield return extension.Item1();
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

            if (metadata.InterfaceType != null)
                if (!interfaceType.IsAssignableFrom(metadata.InterfaceType))
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
