using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

using NXKit.Util;

namespace NXKit.Xml
{

    /// <summary>
    /// Provides various extension methods for working with <see cref="XObject"/> instances.
    /// </summary>
    public static class XObjectExtensions
    {

        /// <summary>
        /// Gets the unique identifier for the object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int GetObjectId(this XObject obj)
        {
            Contract.Requires<ArgumentNullException>(obj != null);
            Contract.Requires<ArgumentNullException>(obj.Document != null);

            // gets the node id, or allocates a new one with the document
            return obj.AnnotationOrCreate<ObjectAnnotation>(() =>
                new ObjectAnnotation(
                    obj.Document.AnnotationOrCreate<DocumentAnnotation>()
                        .GetNextNodeId())).Id;
        }

        /// <summary>
        /// Gets the BaseUri of the <see cref="XObject"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static string BaseUri(this XObject self)
        {
            Contract.Requires<ArgumentNullException>(self != null);

            var baseUriAnno = self.Annotation<BaseUriAnnotation>();
            if (baseUriAnno != null &&
                baseUriAnno.BaseUri != null)
                return baseUriAnno.BaseUri.ToString();

            if (self is XElement)
                return BaseUri((XElement)self);

            return null;
        }

        public static string BaseUri(this XElement self)
        {
            Contract.Requires<ArgumentNullException>(self != null);

            var baseUriAttr = (string)self.Attribute(XNamespace.Xml + "base");
            if (baseUriAttr != null)
                return baseUriAttr;

            if (self.Parent != null)
                return BaseUri(self.Parent);

            if (self.Document != null)
                return BaseUri(self.Document);

            return null;
        }

        /// <summary>
        /// Sets the BaseUri of the <see cref="XObject"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="baseUri"></param>
        public static void BaseUri(this XObject self, Uri baseUri)
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
        public static void BaseUri(this XObject self, string baseUri)
        {
            Contract.Requires<ArgumentNullException>(self != null);

            BaseUri(self, !string.IsNullOrWhiteSpace(baseUri) ? new Uri(baseUri) : null);
        }

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
        /// Gets all the implemented interfaces of this <see cref="XObject"/>.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static IEnumerable<object> Interfaces(this XObject node)
        {
            Contract.Requires<ArgumentNullException>(node != null);

            return Interfaces(node, node.Host().Container);
        }

        /// <summary>
        /// Implements Interfaces, allowing the specification of a container.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        internal static IEnumerable<object> Interfaces(this XObject node, CompositionContainer container)
        {
            Contract.Requires<ArgumentNullException>(node != null);
            Contract.Requires<ArgumentNullException>(container != null);

            return container
                .GetExportedValues<IInterfaceProvider>()
                .SelectMany(i => i.GetInterfaces(node));
        }

        /// <summary>
        /// Gets the implemented interfaces of this <see cref="XObject"/> of the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<object> Interfaces(this XObject node, Type type)
        {
            Contract.Requires<ArgumentNullException>(node != null);

            return Interfaces(node).Where(i => type.IsInstanceOfType(i));
        }

        /// <summary>
        /// Gets the implemented interfaces of the given <see cref="XNode"/> of the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <returns></returns>
        public static IEnumerable<T> Interfaces<T>(this XObject node)
        {
            Contract.Requires<ArgumentNullException>(node != null);

            return Interfaces(node).OfType<T>();
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

            return Interfaces<T>(node).FirstOrDefault();
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
        /// Resolves the <see cref="NXDocumentHost"/> for the given <see cref="XObject"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static NXDocumentHost Host(this XObject self)
        {
            Contract.Requires<ArgumentNullException>(self != null);
            Contract.Ensures(Contract.Result<NXDocumentHost>() != null);

            return self.AnnotationOrCreate<NXDocumentHost>(() => self.Document != null ? self.Document.Host() : null);
        }

    }

}
