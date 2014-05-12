using System;

using NXKit.Composition;

namespace NXKit.XForms
{

    /// <summary>
    /// Captures invocations to handle unwrapping and invoking the deferred update behavior.
    /// </summary>
    [ScopeExport(typeof(IInvokerLayer), Scope.Host)]
    public class DeferredUpdateInvokerLayer :
        IInvokerLayer
    {

        int count = 0;

        public void Invoke(System.Action action)
        {
            try
            {
                count++;
                action();
            }
            finally
            {
                count--;
            }
        }

        public R Invoke<R>(Func<R> func)
        {
            try
            {
                count++;
                return func();
            }
            finally
            {
                count--;
            }
        }

    }

}
