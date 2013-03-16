using System;
using System.Collections.Generic;

namespace XForms.Util
{

    /// <summary>
    /// Provides extension methods for working with dictionaries.
    /// </summary>
    public static class DictionaryExtensions
    {

        public static TValue ValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key)
        {
            TValue v;
            if (source.TryGetValue(key, out v))
                return v;
            else
                return default(TValue);
        }

        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, Func<TValue> func)
        {
            return GetOrCreate(source, key, k => func());
        }

        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, Func<TKey, TValue> func)
        {
            TValue v;
            if (source.TryGetValue(key, out v))
                return v;
            else
                return source[key] = func(key);
        }

    }

}
