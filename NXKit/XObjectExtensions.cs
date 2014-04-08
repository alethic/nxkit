using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

using NXKit.Util;

namespace NXKit
{

    /// <summary>
    /// Provides various extension methods for working with <see cref="XObject"/> instances.
    /// </summary>
    public static class XObjectExtensions
    {

        /// <summary>
        /// Gets all the implemented interfaces of this <see cref="XObject"/>.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static IEnumerable<object> Interfaces(this XObject node)
        {
            Contract.Requires<ArgumentNullException>(node != null);

            return node.Host().Container
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

    }

}
