using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

using NXKit.Util;

namespace NXKit
{

    /// <summary>
    /// Associates serializable state information with <see cref="Visual"/> instances.
    /// </summary>
    [Serializable]
    public class VisualStateCollection :
        ISerializable
    {

        readonly Dictionary<string, Dictionary<Type, object>> store;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public VisualStateCollection()
        {
            this.store = new Dictionary<string, Dictionary<Type, object>>();
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public VisualStateCollection(SerializationInfo info, StreamingContext context)
            : this()
        {
            Contract.Requires<ArgumentNullException>(info != null);

            foreach (var kvp in info)
                store.Add(kvp.Name, (Dictionary<Type, object>)kvp.Value);
        }

        /// <summary>
        /// Gets the state object of the specified type for the specified <see cref="Visual"/>.
        /// </summary>
        /// <param name="visual"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        object Get(ContentVisual visual, Type type, Func<object> getDefaultValue)
        {
            Contract.Requires<ArgumentNullException>(visual != null);
            Contract.Requires<ArgumentNullException>(type != null);
            Contract.Requires<ArgumentNullException>(getDefaultValue != null);

            var visualStore = store.GetOrDefault(visual.UniqueId);
            if (visualStore == null)
                visualStore = store[visual.UniqueId] = new Dictionary<Type, object>();

            var value = visualStore.GetOrDefault(type);
            if (value == null)
                value = visualStore[type] = getDefaultValue();

            return value;
        }

        /// <summary>
        /// Gets the state object of type <typeparamref name="T"/> for <paramref name="visual"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="visual"></param>
        /// <returns></returns>
        public T Get<T>(ContentVisual visual)
            where T : new()
        {
            Contract.Requires<ArgumentNullException>(visual != null);

            return (T)Get(visual, typeof(T), () => new T());
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            foreach (var kvp in store)
                info.AddValue(kvp.Key, kvp.Value);
        }

    }

}
