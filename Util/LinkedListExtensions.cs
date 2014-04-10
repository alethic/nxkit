using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

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
        public static IEnumerable<LinkedListNode<T>> Forwards<T>(this LinkedList<T> self)
        {
            Contract.Requires<ArgumentNullException>(self != null);
            Contract.Requires<ArgumentNullException>(self.First != null);

            return self.First.Forwards();
        }

        /// <summary>
        /// Iterates through the linked list nodes forwards, beginning at the specified node.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static IEnumerable<LinkedListNode<T>> Forwards<T>(this LinkedListNode<T> self)
        {
            Contract.Requires<ArgumentNullException>(self != null);

            yield return self;

            while (self.Next != null)
                yield return self = self.Next;
        }

        /// <summary>
        /// Iterates through the linked list nodes backwards.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static IEnumerable<LinkedListNode<T>> Backwards<T>(this LinkedList<T> self)
        {
            Contract.Requires<ArgumentNullException>(self != null);
            Contract.Requires<ArgumentNullException>(self.First != null);

            return self.Last.Backwards();
        }

        /// <summary>
        /// Iterates through the linked list nodes backwards, beginning at the specified node.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static IEnumerable<LinkedListNode<T>> Backwards<T>(this LinkedListNode<T> self)
        {
            Contract.Requires<ArgumentNullException>(self != null);

            yield return self;

            while (self.Previous != null)
                yield return self = self.Previous;
        }

    }

}
