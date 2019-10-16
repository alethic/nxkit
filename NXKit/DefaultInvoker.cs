using System;
using System.Collections.Generic;

using NXKit.Composition;
using NXKit.Util;

namespace NXKit
{

    [Export(typeof(IInvoker), CompositionScope.Host)]
    public class DefaultInvoker :
        IInvoker
    {

        readonly LinkedList<IExport<IInvokerLayer>> layers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="layers"></param>
        public DefaultInvoker(IEnumerable<IExport<IInvokerLayer>> layers)
        {
            if (layers == null)
                throw new ArgumentNullException(nameof(layers));

            this.layers = layers.ToLinkedList();
        }

        public void Invoke(Action action)
        {
            Invoke(action, layers.First);
        }

        void Invoke(Action action, LinkedListNode<IExport<IInvokerLayer>> next)
        {
            if (next != null)
                next.Value.Value.Invoke(() => Invoke(action, next.Next));
            else
                action();
        }

        public R Invoke<R>(Func<R> func)
        {
            return Invoke(func, layers.First);
        }

        R Invoke<R>(Func<R> func, LinkedListNode<IExport<IInvokerLayer>> next)
        {
            if (next == null && func == null)
                throw new ArgumentException();

            if (next != null)
                return next.Value.Value.Invoke<R>(() => Invoke<R>(func, next.Next));
            else
                return func();
        }

    }

}
