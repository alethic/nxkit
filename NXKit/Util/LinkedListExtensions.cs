using System;
using System.Collections.Generic;

namespace NXKit.Util
{

    public static class LinkedListExtensions
    {

        /// <summary>
        /// Iterates through the linked list nodes forwards.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static IEnumerable<T> Forwards<T>(this LinkedList<T> self)
        {
            if (self == null)
                throw new ArgumentNullException(nameof(self));
            if (self.First == null)
                throw new ArgumentNullException(nameof(self));

            return self.First.Forwards();
        }

        /// <summary>
        /// Iterates through the linked list nodes forwards, beginning at the specified node.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static IEnumerable<T> Forwards<T>(this LinkedListNode<T> self)
        {
            if (self == null)
                throw new ArgumentNullException(nameof(self));

            yield return self.Value;

            while (self.Next != null)
                yield return (self = self.Next).Value;
        }

        /// <summary>
        /// Iterates through the linked list nodes backwards.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static IEnumerable<T> Backwards<T>(this LinkedList<T> self)
        {
            if (self == null)
                throw new ArgumentNullException(nameof(self));
            if (self.First == null)
                throw new ArgumentNullException(nameof(self));

            return self.Last.Backwards();
        }

        /// <summary>
        /// Iterates through the linked list nodes backwards, beginning at the specified node.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static IEnumerable<T> Backwards<T>(this LinkedListNode<T> self)
        {
            if (self == null)
                throw new ArgumentNullException(nameof(self));

            yield return self.Value;

            while (self.Previous != null)
                yield return (self = self.Previous).Value;
        }

    }

}
