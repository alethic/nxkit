using System;
using System.Collections.Generic;

namespace NXKit.Util
{

    /// <summary>
    /// Provides various methods for working with <see cref="Object"/> instances.
    /// </summary>
    public static class ObjectExtensions
    {

        /// <summary>
        /// Yields the instance, then yields each instance returned by applying the step function successively.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        public static IEnumerable<T> Recurse<T>(this T self, Func<T, T> step)
            where T : class
        {
            yield return self;

            foreach (var next in Recurse(step(self), step))
                yield return next;
        }

    }

}
