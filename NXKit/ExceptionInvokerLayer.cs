using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Runtime.ExceptionServices;

namespace NXKit
{

    [InvokerLayer]
    public class ExceptionInvokerLayer :
        IInvokerLayer
    {

        readonly IEnumerable<IExceptionHandler> handlers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="handlers"></param>
        [ImportingConstructor]
        public ExceptionInvokerLayer(
            [ImportMany] IEnumerable<IExceptionHandler> handlers)
        {
            Contract.Requires<ArgumentNullException>(handlers != null);

            this.handlers = handlers;
        }

        public void Invoke(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                HandleException(e);
            }
        }

        public R Invoke<R>(Func<R> func)
        {
            try
            {
                return func();
            }
            catch (Exception e)
            {
                HandleException(e);
            }

            return default(R);
        }

        /// <summary>
        /// Handles an exception by dispatching it to the root <see cref="IExceptionHandler"/>.
        /// </summary>
        /// <param name="exception"></param>
        void HandleException(Exception exception)
        {
            Contract.Requires<ArgumentNullException>(exception != null);

            bool rethrow = true;

            // search for exception handlers
            foreach (var handler in handlers)
                if (handler.HandleException(exception))
                    rethrow = false;

            // should we rethrow the exception?
            if (rethrow)
                ExceptionDispatchInfo.Capture(exception).Throw();
        }

    }

}
