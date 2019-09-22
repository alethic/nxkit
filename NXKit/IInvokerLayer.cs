using System;

namespace NXKit
{

    /// <summary>
    /// Provides a context layer to an invocation within the document context.
    /// </summary>
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

}
