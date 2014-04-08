using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Provides various extension methods for working with <see cref="XObject"/> instances.
    /// </summary>
    public static class XObjectExtensions
    {

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
