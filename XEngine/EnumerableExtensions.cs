using System.Collections.Generic;

namespace XEngine.Forms
{

    /// <summary>
    /// Provides various extensions for enumerables.
    /// </summary>
    static class EnumerableExtensions
    {

        public static IEnumerable<T> Append<T>(this IEnumerable<T> source, T item)
        {
            foreach (var i in source)
                yield return i;
            yield return item;
        }

        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> source, T item)
        {
            yield return item;
            foreach (var i in source)
                yield return i;
        }

    }

}
