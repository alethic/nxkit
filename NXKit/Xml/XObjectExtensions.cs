using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.Util;

namespace NXKit.Xml
{

    /// <summary>
    /// Provides various extension methods for working with <see cref="XObject"/> instances.
    /// </summary>
    public static class XObjectExtensions
    {

        /// <summary>
        /// Resolves a <see cref="XElement"/> by IDREF from the vantage point of this <see cref="XObject"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static XElement ResolveId(this XObject self, string id)
        {
            Contract.Requires<ArgumentNullException>(self != null);
            Contract.Requires<ArgumentNullException>(self.Parent != null);
            Contract.Requires<ArgumentNullException>(id != null);

            var attr = self as XAttribute;
            if (attr != null)
                return attr.ResolveId(id);

            var node = self as XNode;
            if (node != null)
                return node.ResolveId(id);

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets the unique identifier for the object.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static int GetObjectId(this XObject self)
        {
            Contract.Requires<ArgumentNullException>(self != null);
            Contract.Requires<ArgumentNullException>(self.Document != null);

            // gets the node id, or allocates a new one with the document
            return self.AnnotationOrCreate<ObjectAnnotation>(() =>
                new ObjectAnnotation(
                    self.Document.AnnotationOrCreate<DocumentAnnotation>()
                        .GetNextObjectId())).Id;
        }

        class ObjectIdCache
        {

            public Dictionary<int, XObject> cache = new Dictionary<int, XObject>();

        }

        /// <summary>
        /// Locates the object with the given unique identifier.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public static XObject ResolveObjectId(this XObject self, int objectId)
        {
            Contract.Requires<ArgumentNullException>(self != null);
            Contract.Requires<ArgumentNullException>(self.Document != null);

            return self.Document.AnnotationOrCreate<ObjectIdCache>()
                .cache.GetOrAdd(objectId, () =>
                    self.Document.DescendantNodesAndSelf()
                        .FirstOrDefault(i => i.GetObjectId() == objectId));
        }

        #region Naming

        /// <summary>
        /// Gets the namespace associated with a prefix for this <see cref="XObject"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static XNamespace GetNamespaceOfPrefix(this XObject self, string prefix)
        {
            Contract.Requires<ArgumentNullException>(self != null);
            Contract.Requires<ArgumentNullException>(self is XAttribute || self is XNode);
            Contract.Requires<ArgumentException>(self.Parent != null);
            Contract.Requires<ArgumentNullException>(prefix != null);

            var attr = self as XAttribute;
            if (attr != null)
                return attr.GetNamespaceOfPrefix(prefix);

            var node = self as XNode;
            if (node != null)
                return node.GetNamespaceOfPrefix(prefix);

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets the prefix associated with a namespace for this <see cref="XObject"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="ns"></param>
        /// <returns></returns>
        public static string GetPrefixOfNamespace(this XObject self, XNamespace ns)
        {
            Contract.Requires<ArgumentNullException>(self != null);
            Contract.Requires<ArgumentException>(self is XAttribute || self is XNode);
            Contract.Requires<ArgumentException>(self.Parent != null);
            Contract.Requires<ArgumentNullException>(ns != null);

            var attr = self as XAttribute;
            if (attr != null)
                return attr.GetPrefixOfNamespace(ns);

            var node = self as XNode;
            if (node != null)
                return node.GetPrefixOfNamespace(ns);

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Resolves a <see cref="XName"/> from the given prefixed name, given the specified <see cref="XObject"/>'s
        /// naming context.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="prefixedName"></param>
        /// <returns></returns>
        public static XName ResolvePrefixedName(this XObject self, string prefixedName)
        {
            Contract.Requires<ArgumentNullException>(self != null);
            Contract.Requires<ArgumentNullException>(prefixedName != null);

            var i = prefixedName.IndexOf(':');
            if (i == -1)
                return self.GetNamespaceOfPrefix("") + prefixedName;

            var prefix = prefixedName.Substring(0, i);
            if (string.IsNullOrWhiteSpace(prefix))
                prefix = "";

            var localName = prefixedName.Substring(i + 1);
            if (string.IsNullOrWhiteSpace(localName))
                localName = "";

            var ns = self.GetNamespaceOfPrefix(prefix);
            if (ns == null)
                throw new NullReferenceException();

            return ns + localName;
        }

        /// <summary>
        /// Resolves a sequence of <see cref="XName"/>s from the given prefixed names, given the specified <see cref="XObject"/>'s
        /// naming context.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="prefixedNames"></param>
        /// <returns></returns>
        public static IEnumerable<XName> ResolvePrefixedNames(this XObject self, string prefixedNames)
        {
            Contract.Requires<ArgumentNullException>(self != null);
            Contract.Requires<ArgumentNullException>(prefixedNames != null);

            foreach (var v in prefixedNames.Split(' '))
            {
                var name = ResolvePrefixedName(self, v);
                if (name != null)
                    yield return name;
            }
        }

        #endregion

        #region BaseUri

        /// <summary>
        /// Gets the BaseUri of the <see cref="XObject"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static Uri GetBaseUri(this XObject self)
        {
            Contract.Requires<ArgumentNullException>(self != null);

            var baseUriAnno = self.Annotation<BaseUriAnnotation>();
            if (baseUriAnno != null &&
                baseUriAnno.BaseUri != null)
                return baseUriAnno.BaseUri;

            if (self is XElement)
                return GetBaseUri((XElement)self);

            return null;
        }

        /// <summary>
        /// Gets the BaseUri of the <see cref="XElement"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static Uri GetBaseUri(this XElement self)
        {
            Contract.Requires<ArgumentNullException>(self != null);

            var baseUriAttr = (string)self.Attribute(XNamespace.Xml + "base");
            if (baseUriAttr != null)
                return new Uri(baseUriAttr, UriKind.RelativeOrAbsolute);

            if (self.Parent != null)
                return GetBaseUri(self.Parent);

            if (self.Document != null)
                return GetBaseUri(self.Document);

            return null;
        }

        /// <summary>
        /// Sets the BaseUri of the <see cref="XObject"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="baseUri"></param>
        public static void SetBaseUri(this XObject self, Uri baseUri)
        {
            Contract.Requires<ArgumentNullException>(self != null);

            if (baseUri != null)
                self.AnnotationOrCreate<BaseUriAnnotation>().BaseUri = baseUri;
            else
                self.RemoveAnnotations<BaseUriAnnotation>();
        }

        /// <summary>
        /// Sets the BaseUri of the <see cref="XObject"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="baseUri"></param>
        public static void SetBaseUri(this XObject self, string baseUri)
        {
            Contract.Requires<ArgumentNullException>(self != null);

            SetBaseUri(self, !string.IsNullOrWhiteSpace(baseUri) ? new Uri(baseUri) : null);
        }

        #endregion

        #region Navigation

        /// <summary>
        /// Obtains the ancestors of the given <see cref="XObject"/>
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static IEnumerable<XElement> Ancestors(this XObject self)
        {
            Contract.Requires<ArgumentNullException>(self != null);

            return self.Parent != null ? self.Parent.AncestorsAndSelf() : Enumerable.Empty<XElement>();
        }

        /// <summary>
        /// Obtains the ancestors of the given <see cref="XObject"/> filtered by name.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IEnumerable<XElement> Ancestors(this XObject self, XName name)
        {
            Contract.Requires<ArgumentNullException>(self != null);
            Contract.Requires<ArgumentNullException>(name != null);

            return self.Parent != null ? self.Parent.AncestorsAndSelf(name) : Enumerable.Empty<XElement>();
        }

        /// <summary>
        /// 
        /// Obtains the ancestors of the given <see cref="XObject"/>, including the specified instance.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static IEnumerable<XObject> AncestorsAndSelf(this XObject self)
        {
            Contract.Requires<ArgumentNullException>(self != null);

            return self.Ancestors().Prepend(self);
        }

        #endregion

        #region Annotations

        /// <summary>
        /// Gets the first annotation object of the specified type, or creates a new one.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static T AnnotationOrCreate<T>(this XObject self)
            where T : class, new()
        {
            Contract.Requires<ArgumentNullException>(self != null);

            var value = self.Annotation<T>();
            if (value == null)
                self.AddAnnotation(value = new T());

            return value;
        }

        /// <summary>
        /// Gets the first annotation object of the specified type, or creates a new one.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static T AnnotationOrCreate<T>(this XObject self, Func<T> create)
            where T : class
        {
            Contract.Requires<ArgumentNullException>(self != null);
            Contract.Requires<ArgumentNullException>(create != null);

            var value = self.Annotation<T>();
            if (value == null)
                self.AddAnnotation(value = create());

            return value;
        }

        /// <summary>
        /// Gets the first annotation object of the specified type, or creates a new one.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object AnnotationOrCreate(this XObject self, Type type)
        {
            Contract.Requires<ArgumentNullException>(self != null);
            Contract.Requires<ArgumentNullException>(type != null);

            var value = self.Annotation(type);
            if (value == null)
                self.AddAnnotation(value = Activator.CreateInstance(type));

            return value;
        }

        /// <summary>
        /// Gets the first annotation object of the specified type, or creates a new one.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="type"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public static object AnnotationOrCreate(this XObject self, Type type, Func<object> create)
        {
            Contract.Requires<ArgumentNullException>(self != null);
            Contract.Requires<ArgumentNullException>(type != null);
            Contract.Requires<ArgumentNullException>(create != null);

            var value = self.Annotation(type);
            if (value == null)
                self.AddAnnotation(value = create());

            return value;
        }

        #endregion

        #region NXKit

        /// <summary>
        /// Gets all the implemented interfaces of this <see cref="XObject"/>.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static IEnumerable<object> Interfaces(this XObject node, Type type)
        {
            Contract.Requires<ArgumentNullException>(node != null);
            Contract.Requires<ArgumentNullException>(type != null);

            return Interfaces(node, type, node.Exports());
        }

        /// <summary>
        /// Gets all the implemented interfaces of this <see cref="XObject"/>.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static IEnumerable<T> Interfaces<T>(this XObject node)
        {
            Contract.Requires<ArgumentNullException>(node != null);

            return Interfaces<T>(node, node.Exports());
        }

        /// <summary>
        /// Implements Interfaces, allowing the specification of an export provider.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="exports"></param>
        /// <returns></returns>
        static IEnumerable<object> Interfaces(this XObject node, Type type, ExportProvider exports)
        {
            Contract.Requires<ArgumentNullException>(node != null);
            Contract.Requires<ArgumentNullException>(type != null);
            Contract.Requires<ArgumentNullException>(exports != null);

            return (ExtensionQuery)node.AnnotationOrCreate(typeof(ExtensionQuery<>).MakeGenericType(type), () => exports.GetExportedValue(typeof(ExtensionQuery<>).MakeGenericType(type)));
        }

        /// <summary>
        /// Implements Interfaces, allowing the specification of an export provider.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="exports"></param>
        /// <returns></returns>
        static IEnumerable<T> Interfaces<T>(this XObject node, ExportProvider exports)
        {
            Contract.Requires<ArgumentNullException>(node != null);
            Contract.Requires<ArgumentNullException>(exports != null);

            return node.AnnotationOrCreate<ExtensionQuery<T>>(() => exports.GetExportedValue<ExtensionQuery<T>>());
        }

        /// <summary>
        /// Gets the specific interface of this <see cref="XObject"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <returns></returns>
        public static T InterfaceOrDefault<T>(this XObject node)
        {
            Contract.Requires<ArgumentNullException>(node != null);

            return Interfaces<T>(node)
                .FirstOrDefault();
        }

        /// <summary>
        /// Gets the specific interface of this <see cref="XObject"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <returns></returns>
        public static T Interface<T>(this XObject node)
        {
            Contract.Requires<ArgumentNullException>(node != null);
            Contract.Ensures(Contract.Result<T>() != null);

            var i = InterfaceOrDefault<T>(node);
            if (i == null)
                throw new NullReferenceException();

            return i;
        }

        /// <summary>
        /// Gets the <see cref="ExportProvider"/> for the given <see cref="XObject"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static ExportProvider Exports(this XObject self)
        {
            Contract.Requires<ArgumentNullException>(self != null);
            Contract.Ensures(Contract.Result<ExportProvider>() != null);

            // get or create the new object container annotation
            return self.AnnotationOrCreate<ExportProvider>(() =>
            {
                var document = self.Document.Annotation<Document>();
                if (document == null)
                    throw new InvalidOperationException();

                // initialize new container
                var container = CompositionUtil.ConfigureContainer(new CompositionContainer(
                    document.Configuration.ObjectCatalog,
                    CompositionOptions.DisableSilentRejection,
                    document.Container));

                if (self is XObject)
                    container.WithExport<XObject>(self);
                if (self is XDocument)
                    container.WithExport<XDocument>((XDocument)self);
                if (self is XElement)
                    container.WithExport<XElement>((XElement)self);
                if (self is XNode)
                    container.WithExport<XNode>((XNode)self);
                if (self is XAttribute)
                    container.WithExport<XAttribute>((XAttribute)self);

                return container;
            });
        }

        #endregion

        /// <summary>
        /// Clones the specified <see cref="XObject"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static XObject Clone(this XObject self)
        {
            Contract.Requires<ArgumentNullException>(self != null);

            return XCloneTransformer.Default.Visit(self);
        }

    }

}
