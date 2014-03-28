using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

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
        /// Gets the interface of the specified type, or creats a new instance with the given function.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        protected T GetOrCreate<T>(NXNode node, Func<T> func)
        {
            Contract.Requires<ArgumentNullException>(node != null);
            Contract.Requires<ArgumentNullException>(func != null);

            var i = node.Annotation<T>();
            if (i == null)
                node.AddAnnotation(i = func());

            return i;
        }

        public abstract IEnumerable<object> GetInterfaces(NXNode node);

    }

}
