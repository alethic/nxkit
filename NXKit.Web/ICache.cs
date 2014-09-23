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
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="remove"></param>
        /// <returns></returns>
        T Get<T>(string key, bool remove)
            where T : class;

        /// <summary>
        /// Sets the value into the cache by it's key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Add<T>(string key, T value)
            where T : class;

    }

}
