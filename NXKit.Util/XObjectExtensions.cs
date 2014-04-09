using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

namespace NXKit.Util
{

    /// <summary>
    /// Provides extension methods for working with <see cref="XObject"/> instances.
    /// </summary>
    public static class XObjectExtensions
    {

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

            var baseUriAttr = (string)self.Attribute("xml:base");
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

    }

}
