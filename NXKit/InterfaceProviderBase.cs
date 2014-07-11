using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Provides a base <see cref="IExtensionProvider"/> implementation that supports caching implementations in
    /// the annotations collection.
    /// </summary>
    public abstract class InterfaceProviderBase :
        IExtensionProvider
    {

        /// <summary>
        /// Gets the interface of the specified type, if it's already created.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected object Get(XObject obj, Type type)
        {
            Contract.Requires<ArgumentNullException>(obj != null);
            Contract.Requires<ArgumentNullException>(type != null);

            return obj.Annotation(type);
        }

        /// <summary>
        /// Gets the interface of the specified type, if it's already created.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected T Get<T>(XObject obj)
            where T : class
        {
            Contract.Requires<ArgumentNullException>(obj != null);

            return obj.Annotation<T>();
        }

        /// <summary>
        /// Creates an interface of the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        protected object CreateAndAdd(XObject obj, Type type, Func<object> func)
        {
            Contract.Requires<ArgumentNullException>(obj != null);
            Contract.Requires<ArgumentNullException>(type != null);
            Contract.Requires<ArgumentNullException>(func != null);

            // create new instance
            var i = func();
            if (i == null)
                throw new NullReferenceException("Interface creation function returned null.");

            // add new instance
            obj.AddAnnotation(i);

            return i;
        }

        /// <summary>
        /// Creates an interface of the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        protected T CreateAndAdd<T>(XObject obj, Func<T> func)
            where T : class
        {
            Contract.Requires<ArgumentNullException>(obj != null);
            Contract.Requires<ArgumentNullException>(func != null);

            return CreateAndAdd(obj, () => (T)func());
        }

        /// <summary>
        /// Gets the interface of the specified type, or creats a new instance with the given function.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        protected object GetOrCreate(XObject obj, Type type, Func<object> func)
        {
            Contract.Requires<ArgumentNullException>(obj != null);
            Contract.Requires<ArgumentNullException>(type != null);
            Contract.Requires<ArgumentNullException>(func != null);

            return Get(obj, type) ?? CreateAndAdd(obj, type, func);
        }

        /// <summary>
        /// Gets the interface of the specified type, or creats a new instance with the given function.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        protected T GetOrCreate<T>(XObject obj, Func<T> func)
            where T : class
        {
            Contract.Requires<ArgumentNullException>(obj != null);
            Contract.Requires<ArgumentNullException>(func != null);

            return (T)GetOrCreate(obj, typeof(T), () => func());
        }

        /// <summary>
        /// Implement this method to handle retrieving interfaces for the specified node.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract IEnumerable<T> GetExtensions<T>(XObject obj);

        /// <summary>
        /// Implement this method to handle retrieving interfaces for the specified node.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public abstract IEnumerable<object> GetExtensions(XObject obj, Type type);

    }

}
