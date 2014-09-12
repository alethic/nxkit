using System;

namespace NXKit.Web
{

    /// <summary>
    /// Provides the ability to resolve <see cref="Object"/> instances given a key.
    /// </summary>
    public interface ICache
    {

        /// <summary>
        /// Gets a matching value from the cache if available.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object Get(string key);

        /// <summary>
        /// Sets the value into the cache by it's key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Add(string key, object value);

    }

}
