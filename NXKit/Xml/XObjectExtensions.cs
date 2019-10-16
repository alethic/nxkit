using System;
using System.Collections.Generic;
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
            if (self == null)
                throw new ArgumentNullException(nameof(self));
            if (self.Parent == null)
                throw new ArgumentNullException(nameof(self));
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            if (self is XAttribute attr)
                return attr.ResolveId(id);

            if (self is XNode node)
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
            if (self == null)
                throw new ArgumentNullException(nameof(self));
            if (self.Document == null)
                throw new ArgumentNullException(nameof(self));

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
            if (self == null)
                throw new ArgumentNullException(nameof(self));
            if (self.Document == null)
                throw new ArgumentNullException(nameof(self));

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
            if (self == null)
                throw new ArgumentNullException(nameof(self));
            if (self.Parent == null)
                throw new ArgumentNullException(nameof(self));
            if (prefix == null)
                throw new ArgumentNullException(nameof(prefix));

            if (self is XAttribute attr)
                return attr.GetNamespaceOfPrefix(prefix);

            if (self is XNode node)
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
            if (self == null)
                throw new ArgumentNullException(nameof(self));
            if (self.Parent == null)
                throw new ArgumentNullException(nameof(self));
            if (ns == null)
                throw new ArgumentNullException(nameof(ns));

            if (self is XAttribute attr)
                return attr.GetPrefixOfNamespace(ns);

            if (self is XNode node)
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
            if (self == null)
                throw new ArgumentNullException(nameof(self));
            if (prefixedName == null)
                throw new ArgumentNullException(nameof(prefixedName));

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
            if (self == null)
                throw new ArgumentNullException(nameof(self));
            if (prefixedNames == null)
                throw new ArgumentNullException(nameof(prefixedNames));

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
            if (self == null)
                throw new ArgumentNullException(nameof(self));

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
            if (self == null)
                throw new ArgumentNullException(nameof(self));

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
            if (self == null)
                throw new ArgumentNullException(nameof(self));

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
            if (self == null)
                throw new ArgumentNullException(nameof(self));

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
            if (self == null)
                throw new ArgumentNullException(nameof(self));

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
            if (self == null)
                throw new ArgumentNullException(nameof(self));
            if (name == null)
                throw new ArgumentNullException(nameof(name));

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
            if (self == null)
                throw new ArgumentNullException(nameof(self));

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
            if (self == null)
                throw new ArgumentNullException(nameof(self));

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
            if (self == null)
                throw new ArgumentNullException(nameof(self));
            if (create == null)
                throw new ArgumentNullException(nameof(create));

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
            if (self == null)
                throw new ArgumentNullException(nameof(self));
            if (type == null)
                throw new ArgumentNullException(nameof(type));

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
            if (self == null)
                throw new ArgumentNullException(nameof(self));
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (create == null)
                throw new ArgumentNullException(nameof(create));

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
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return Interfaces(node, type, node.GetContext());
        }

        /// <summary>
        /// Gets all the implemented interfaces of this <see cref="XObject"/>.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static IEnumerable<T> Interfaces<T>(this XObject node)
            where T : class, IExtension
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            return Interfaces<T>(node, node.GetContext());
        }

        /// <summary>
        /// Implements Interfaces, allowing the specification of an export provider.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        static IEnumerable<object> Interfaces(this XObject node, Type type, ICompositionContext context)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var q = typeof(ExtensionQuery<>).MakeGenericType(type);
            return (ExtensionQuery)node.AnnotationOrCreate(q, () => (ExtensionQuery)context.Resolve(q));
        }

        /// <summary>
        /// Implements Interfaces, allowing the specification of an export provider.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        static IEnumerable<T> Interfaces<T>(this XObject node, ICompositionContext context)
            where T : class, IExtension
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return node.AnnotationOrCreate<ExtensionQuery<T>>(() => context.Resolve<ExtensionQuery<T>>());
        }

        /// <summary>
        /// Gets the specific interface of this <see cref="XObject"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <returns></returns>
        public static T InterfaceOrDefault<T>(this XObject node)
            where T : class, IExtension
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

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
            where T : class, IExtension
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

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
        public static ICompositionContext GetContext(this XObject self)
        {
            if (self == null)
                throw new ArgumentNullException(nameof(self));

            // get or create the new object container annotation
            return self.AnnotationOrCreate(() =>
            {
                var document = self.Document.Annotation<Document>();
                if (document == null)
                    throw new InvalidOperationException();

                // initialize new container
                var context = document.Context.BeginContext(CompositionScope.Object, b =>
                {
                    if (self is XObject)
                        b.AddInstance<XObject>(self);
                    if (self is XDocument)
                        b.AddInstance<XDocument>((XDocument)self);
                    if (self is XElement)
                        b.AddInstance<XElement>((XElement)self);
                    if (self is XNode)
                        b.AddInstance<XNode>((XNode)self);
                    if (self is XAttribute)
                        b.AddInstance<XAttribute>((XAttribute)self);
                });

                return context;
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
            if (self == null)
                throw new ArgumentNullException(nameof(self));

            return XCloneTransformer.Default.Visit(self);
        }

    }

}
