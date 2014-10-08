using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace NXKit.Util
{

    public static class QueueExtensions
    {

        /// <summary>
        /// Returns an enumeration that dequeues items from the <see cref="Queue"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static IEnumerable<T> DequeueAll<T>(this Queue<T> self)
        {
            Contract.Requires<ArgumentNullException>(self != null);

            while (self.Count > 0)
                yield return self.Dequeue();
        }

    }

}
