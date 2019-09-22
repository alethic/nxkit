using System;
using System.Collections.Generic;

namespace NXKit.Util
{

    /// <summary>
    /// Provides various extensions for enumerables.
    /// </summary>
    public static class EnumerableExtensions
    {

        /// <summary>
        /// Appends <paramref name="item"/> to <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static IEnumerable<T> Append<T>(this IEnumerable<T> source, T item)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            foreach (var i in source)
                yield return i;
            yield return item;
        }

        /// <summary>
        /// Prepends <paramref name="item"/> to <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> source, T item)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            yield return item;
            foreach (var i in source)
                yield return i;
        }

        public static IEnumerable<T> Recurse<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> func)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            foreach (var i in source)
            {
                yield return i;
                foreach (var j in Recurse(func(i), func))
                    yield return j;
            }
        }

        /// <summary>
        /// Creates a <see cref="LinkedList{T}"/> from a <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static LinkedList<T> ToLinkedList<T>(this IEnumerable<T> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new LinkedList<T>(source);
        }

        /// <summary>
        /// Wraps the given enumerable with another enumerable that can be enumerated multiple times without
        /// reenumerating the original.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<T> Buffer<T>(this IEnumerable<T> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return BufferInternal<T>(source.GetEnumerator(), new LinkedList<T>());
        }

        /// <summary>
        /// Generator for Tee.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="cache"></param>
        /// <returns></returns>
        static IEnumerable<T> BufferInternal<T>(this IEnumerator<T> source, LinkedList<T> cache)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (cache == null)
                throw new ArgumentNullException(nameof(cache));

            var node = cache.First;
            if (node != null)
                yield return node.Value;

            while (true)
            {
                // fill from enumerator if we haven't done so already
                if (node == null || node == cache.Last)
                {
                    if (source.MoveNext())
                        cache.AddLast(source.Current);
                    else
                        yield break;
                }

                node = node != null ? node.Next : cache.First;
                if (node == null)
                    yield break;

                // yield value from cache and advance
                yield return node.Value;
            }
        }

    }

}
