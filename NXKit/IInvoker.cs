using System;

namespace NXKit
{

    /// <summary>
    /// Invokes a given function within a context.
    /// </summary>
    public interface IInvoker
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
