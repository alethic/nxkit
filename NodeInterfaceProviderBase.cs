using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Provides a base <see cref="INodeInterfaceProvider"/> implementation that supports caching implementations in
    /// the annotations collection.
    /// </summary>
    public abstract class NodeInterfaceProviderBase :
        INodeInterfaceProvider
    {

        /// <summary>
        /// Gets the interface of the specified type, if it's already created.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected object Get(XNode node, Type type)
        {
            Contract.Requires<ArgumentNullException>(node != null);
            Contract.Requires<ArgumentNullException>(type != null);

            return node.Annotation(type);
        }

        /// <summary>
        /// Gets the interface of the specified type, if it's already created.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <returns></returns>
        protected T Get<T>(XNode node)
            where T : class
        {
            Contract.Requires<ArgumentNullException>(node != null);

            return node.Annotation<T>();
        }

        /// <summary>
        /// Creates an interface of the specified 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        protected object CreateAndAdd(XNode node, Type type, Func<object> func)
        {
            Contract.Requires<ArgumentNullException>(node != null);
            Contract.Requires<ArgumentNullException>(type != null);
            Contract.Requires<ArgumentNullException>(func != null);

            // create new instance
            var i = func();
            if (i == null)
                throw new NullReferenceException("Interface creation function returned null.");

            // add new instance
            node.AddAnnotation(i);

            return i;
        }

        /// <summary>
        /// Creates an interface of the specified 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        protected T CreateAndAdd<T>(XNode node, Func<T> func)
            where T : class
        {
            Contract.Requires<ArgumentNullException>(node != null);
            Contract.Requires<ArgumentNullException>(func != null);

            return CreateAndAdd(node, () => (T)func());
        }

        /// <summary>
        /// Gets the interface of the specified type, or creats a new instance with the given function.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="node"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        protected object GetOrCreate(XNode node, Type type, Func<object> func)
        {
            Contract.Requires<ArgumentNullException>(node != null);
            Contract.Requires<ArgumentNullException>(type != null);
            Contract.Requires<ArgumentNullException>(func != null);

            return Get(node, type) ?? CreateAndAdd(node, type, func);
        }

        /// <summary>
        /// Gets the interface of the specified type, or creats a new instance with the given function.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        protected T GetOrCreate<T>(XNode node, Func<T> func)
            where T : class
        {
            Contract.Requires<ArgumentNullException>(node != null);
            Contract.Requires<ArgumentNullException>(func != null);

            return (T)GetOrCreate(node, typeof(T), () => func());
        }

        /// <summary>
        /// Implement this method to handle retrieving interfaces for the specified node.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public abstract IEnumerable<object> GetInterfaces(XNode node);

    }

}
