using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;

using NXKit.Composition;
using NXKit.Xml;

namespace NXKit.XForms
{

    /// <summary>
    /// Captures invocations to handle unwrapping and invoking the deferred update behavior.
    /// </summary>
    [InvokerLayer]
    public class DeferredUpdateInvokerLayer :
        IInvokerLayer
    {

        readonly Func<NXDocumentHost> host;
        int count = 0;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="host"></param>
        [ImportingConstructor]
        public DeferredUpdateInvokerLayer(
            [Import] Func<NXDocumentHost> host)
        {
            Contract.Requires<ArgumentNullException>(host != null);

            this.host = host;
        }

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

                if (count == 0)
                    InvokeDeferredUpdates();
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

                if (count == 0)
                    InvokeDeferredUpdates();
            }
        }

        /// <summary>
        /// Invoke any outstanding model updates.
        /// </summary>
        void InvokeDeferredUpdates()
        {
            var models = host().Xml
                .Descendants(Constants.XForms_1_0 + "model")
                .Select(i => i.Interface<Model>());

            foreach (var model in models)
                model.InvokeDeferredUpdates();
        }

    }

}
