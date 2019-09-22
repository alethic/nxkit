using System;
using System.Collections.Generic;

namespace NXKit.Util
{

    /// <summary>
    /// Provides extension methods for working with dictionaries.
    /// </summary>
    public static class DictionaryExtensions
    {

        /// <summary>
        /// Gets the given key vaue from the dictionary, or the specified value if it is not found.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TValue GetOrValue<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            TValue v;
            if (source.TryGetValue(key, out v))
                return v;
            else
                return value;
        }

        /// <summary>
        /// Gets the given key value from the dictionary, or the default for the type.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return GetOrValue<TKey, TValue>(source, key, default(TValue));
        }

        /// <summary>
        /// Gets the given key value from the dictionary, or gets the value from the function and adds it.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, Func<TValue> func)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return GetOrAdd(source, key, k => func());
        }

        /// <summary>
        /// Gets the given key value from the dictionary, or gets the value from the function and adds it.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, Func<TKey, TValue> func)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            TValue v;
            if (source.TryGetValue(key, out v))
                return v;
            else
                return source[key] = func(key);
        }

    }

}
