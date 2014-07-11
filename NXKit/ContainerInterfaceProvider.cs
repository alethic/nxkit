using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.Util;

namespace NXKit
{

    /// <summary>
    /// Provides interface lookup using the container.
    /// </summary>
    [Export(typeof(IInterfaceProvider))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class ContainerInterfaceProvider :
        IInterfaceProvider
    {

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
        public ContainerInterfaceProvider(
            XObject obj,
            [ImportMany] IEnumerable<Lazy<IExtension, IDictionary<string, object>>> extensions)
        {
            Contract.Requires<ArgumentNullException>(obj != null);
            Contract.Requires<ArgumentNullException>(extensions != null);

            this.obj = obj;
            this.extensions = extensions
                .Select(i => Tuple.Create(
                    GetLazyFunc(i),
                    ExtensionMetadata.Extract(i.Metadata)))
                .ToList()
                .AsEnumerable();
        }

        public IEnumerable<T> GetInterfaces<T>(XObject obj)
        {
            return GetExtensionsInternal(obj, typeof(T)).OfType<T>();
        }

        public IEnumerable<object> GetInterfaces(XObject obj, Type type)
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
