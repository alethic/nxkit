using System;
using System.Collections.Generic;

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
            if (self == null)
                throw new ArgumentNullException(nameof(self));

            while (self.Count > 0)
                yield return self.Dequeue();
        }

    }

}
