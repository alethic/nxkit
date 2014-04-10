using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace NXKit.Util
{

    /// <summary>
    /// Provides various extensions for enumerables.
    /// </summary>
    public static class EnumerableExtensions
    {

        public static IEnumerable<T> Append<T>(this IEnumerable<T> source, T item)
        {
            Contract.Requires<ArgumentNullException>(source != null);

            foreach (var i in source)
                yield return i;
            yield return item;
        }

        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> source, T item)
        {
            Contract.Requires<ArgumentNullException>(source != null);

            yield return item;
            foreach (var i in source)
                yield return i;
        }

        public static IEnumerable<T> Recurse<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> func)
        {
            Contract.Requires<ArgumentNullException>(source != null);

            foreach (var i in source)
            {
                yield return i;
                foreach (var j in Recurse(func(i), func))
                    yield return j;
            }
        }

        public static IEnumerable<T> DistinctByReference<T>(this IEnumerable<T> source)
            where T : class
        {
            Contract.Requires<ArgumentNullException>(source != null);

            return source.Distinct<T>(EqualityComparer<object>.Default);
        }

    }

}
