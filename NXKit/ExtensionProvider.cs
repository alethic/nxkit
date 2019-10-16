using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit
{

    /// <summary>
    /// Provides interface lookup using the container.
    /// </summary>
    [Export(typeof(ExtensionProvider<>), CompositionScope.Object)]
    public class ExtensionProvider<T> :
        ExtensionProvider
        where T : class
    {

        readonly IEnumerable<IExport<T, IExtensionMetadata>> extensions;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="provider"></param>
        public ExtensionProvider(XObject obj, IEnumerable<IExport<T, IExtensionMetadata>> extensions) :
            base(obj)
        {
            this.extensions = extensions ?? throw new ArgumentNullException(nameof(extensions));
        }

        /// <summary>
        /// Exports typed extensions.
        /// </summary>
        public IEnumerable<T> Extensions => extensions.Where(i => Predicate(obj, i.Metadata)).Select(i => i.Value);

    }

    /// <summary>
    /// Queries for <see cref="IExtension"/> implementors.
    /// </summary>
    public abstract class ExtensionProvider
    {

        static readonly ConcurrentDictionary<Type, IExtensionPredicate> predicates = new ConcurrentDictionary<Type, IExtensionPredicate>();

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

        protected readonly XObject obj;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="obj"></param>
        protected ExtensionProvider(XObject obj)
        {
            this.obj = obj ?? throw new ArgumentNullException(nameof(obj));
        }

    }

}
