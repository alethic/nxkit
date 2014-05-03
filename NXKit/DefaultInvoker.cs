using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;

using NXKit.Composition;

namespace NXKit
{

    [ScopeExport(typeof(IInvoker), Scope.Host)]
    public class DefaultInvoker :
        IInvoker
    {

        readonly LinkedList<IInvokerLayer> layers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="layers"></param>
        [ImportingConstructor]
        public DefaultInvoker(
            [ImportMany] IEnumerable<IInvokerLayer> layers)
        {
            Contract.Requires<ArgumentNullException>(layers != null);

            this.layers = new LinkedList<IInvokerLayer>(layers);
        }

        public void Invoke(Action action)
        {
            Invoke(action, layers.First);
        }

        void Invoke(Action action, LinkedListNode<IInvokerLayer> next)
        {
            if (next != null)
                next.Value.Invoke(() => Invoke(action, next.Next));
            else
                action();
        }

        public R Invoke<R>(Func<R> func)
        {
            return Invoke(func, layers.First);
        }

        R Invoke<R>(Func<R> func, LinkedListNode<IInvokerLayer> next)
        {
            if (next != null)
                return next.Value.Invoke<R>(() => Invoke<R>(func, next.Next));
            else
                return func();
        }

    }

}
