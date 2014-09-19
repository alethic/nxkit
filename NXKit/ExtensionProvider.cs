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
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class ExtensionProvider<T>
        where T : class
    {

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
            Contract.Requires<ArgumentNullException>(obj != null);
            Contract.Requires<ArgumentNullException>(extensions != null);

            this.obj = obj;
            this.extensions = GetExtensions(extensions).ToList();
        }

        /// <summary>
        /// Exports typed extensions.
        /// </summary>
        [Export(typeof(Extension<>))]
        public Extension<T> Extension
        {
            get { return new Extension<T>(() => extensions.Select(i => i.Value).OfType<T>().FirstOrDefault()); }
        }

        /// <summary>
        /// Exports typed extensions.
        /// </summary>
        [Export(typeof(ExtensionQuery<>))]
        public ExtensionQuery<T> ExtensionQuery
        {
            get { return new ExtensionQuery<T>(() => extensions.Select(i => i.Value).OfType<T>()); }
        }

        /// <summary>
        /// Returns each <see cref="IExtension"/> implementation supported on this object.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Lazy<IExtension>> GetExtensions(IEnumerable<Lazy<IExtension, IDictionary<string, object>>> extensions)
        {
            foreach (var extension in extensions)
                foreach (var metadata in ExtensionMetadata.Extract(extension.Metadata))
                    if (Predicate(obj, metadata))
                        yield return extension;
        }

        /// <summary>
        /// Tests a given extension metadata set against the given object.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        bool Predicate(XObject obj, IExtensionMetadata metadata)
        {
            if (!metadata.ObjectType.HasFlag(GetObjectType(obj)))
                return false;

            if (obj is XElement)
                if (!IsMatch((XElement)obj, metadata.NamespaceName, metadata.LocalName))
                    return false;

            if (obj is XAttribute)
                if (!IsMatch((XAttribute)obj, metadata.NamespaceName, metadata.LocalName))
                    return false;

            // test against specified predicate type
            var predicate = metadata.PredicateType != null ? (IExtensionPredicate)Activator.CreateInstance(metadata.PredicateType) : null;
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
