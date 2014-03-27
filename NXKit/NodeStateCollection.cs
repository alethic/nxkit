using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

using NXKit.Util;

namespace NXKit
{

    /// <summary>
    /// Associates serializable state information with <see cref="NXNode"/> instances.
    /// </summary>
    [Serializable]
    public class NodeStateCollection :
        ISerializable
    {

        readonly Dictionary<string, LinkedList<object>> store;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public NodeStateCollection()
        {
            this.store = new Dictionary<string, LinkedList<object>>();
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public NodeStateCollection(SerializationInfo info, StreamingContext context)
            : this()
        {
            Contract.Requires<ArgumentNullException>(info != null);

            foreach (var kvp in info)
                store.Add(kvp.Name, (LinkedList<object>)kvp.Value);
        }

        /// <summary>
        /// Gets the state object of the specified type for the specified <see cref="NXNode"/>.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public LinkedList<object> Get(NXElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            var visualStore = store.GetOrDefault(element.UniqueId);
            if (visualStore == null)
                visualStore = store[element.UniqueId] = new LinkedList<object>();

            return visualStore;
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            foreach (var kvp in store)
                info.AddValue(kvp.Key, kvp.Value);
        }

    }

}
