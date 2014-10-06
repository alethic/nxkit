using System;
using System.Diagnostics.Contracts;

namespace NXKit
{

    /// <summary>
    /// Provides a context layer to an invocation within the document context.
    /// </summary>
    [ContractClass(typeof(IInvokerLayer_Contract))]
    public interface IInvokerLayer
    {

        /// <summary>
        /// Invokes the given <see cref="Action"/> within the document context.
        /// </summary>
        /// <param name="action"></param>
        void Invoke(Action action);

        /// <summary>
        /// Invokes the given <see cref="Func`1"/> within the document context.
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        R Invoke<R>(Func<R> func);

    }

    [ContractClassFor(typeof(IInvokerLayer))]
    public abstract class IInvokerLayer_Contract :
        IInvokerLayer
    {

        public void Invoke(Action action)
        {
            Contract.Requires<ArgumentNullException>(action != null);
            throw new NotImplementedException();
        }

        public R Invoke<R>(Func<R> func)
        {
            Contract.Requires<ArgumentNullException>(func != null);
            throw new NotImplementedException();
        }

    }

}
