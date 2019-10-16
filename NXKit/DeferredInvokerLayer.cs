using System;
using System.Linq;
using System.Xml;

using NXKit.Composition;
using NXKit.Util;
using NXKit.Xml;

namespace NXKit
{

    /// <summary>
    /// Captures invocations to handle unwrapping and invoking the deferred behavior.
    /// </summary>
    [Export(typeof(IInvokerLayer), CompositionScope.Host)]
    public class DeferredInvokerLayer :
        IInvokerLayer
    {

        readonly Func<Document> host;
        readonly IExport<IInvoker> invoker;
        int count = 0;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="invoker"></param>
        public DeferredInvokerLayer(Func<Document> host, IExport<IInvoker> invoker)
        {
            this.host = host ?? throw new ArgumentNullException(nameof(host));
            this.invoker = invoker ?? throw new ArgumentNullException(nameof(invoker));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="environment"></param>
        /// <param name="invoker"></param>
        public DeferredInvokerLayer(DocumentEnvironment environment, IExport<IInvoker> invoker) : this(() => environment?.GetHost(), invoker)
        {

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
                if (--count == 0)
                    DeferredBehavior();
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
                if (--count == 0)
                    DeferredBehavior();
            }
        }

        /// <summary>
        /// Invoke any outstanding model updates.
        /// </summary>
        void DeferredBehavior()
        {
            DeferredInit();
            DeferredLoad();
            DeferredInvoke();
        }

        void DeferredInit()
        {
            try
            {
                count++;

                while (true)
                {
                    var inits = host().Xml
                        .DescendantNodesAndSelf()
                        .Where(i => i.NodeType == XmlNodeType.Document || i.NodeType == XmlNodeType.Element)
                        .Where(i => i.InterfaceOrDefault<IOnInit>() != null)
                        .Where(i => i.AnnotationOrCreate<NXObjectAnnotation>().Init == true)
                        .ToLinkedList();

                    if (inits.Count == 0)
                        break;

                    foreach (var init in inits)
                        if (init.Document != null)
                        {
                            invoker.Value.Invoke(() => init.Interface<IOnInit>().Init());
                            init.AnnotationOrCreate<NXObjectAnnotation>().Init = false;
                        }
                }
            }
            finally
            {
                count--;
            }
        }

        void DeferredLoad()
        {
            try
            {
                count++;

                while (true)
                {
                    var loads = host().Xml
                        .DescendantNodesAndSelf()
                        .Where(i => i.NodeType == XmlNodeType.Document || i.NodeType == XmlNodeType.Element)
                        .Where(i => i.InterfaceOrDefault<IOnLoad>() != null)
                        .Where(i => i.AnnotationOrCreate<NXObjectAnnotation>().Load == true)
                        .ToLinkedList();

                    if (loads.Count == 0)
                        break;

                    foreach (var load in loads)
                        if (load.Document != null)
                        {
                            invoker.Value.Invoke(() => load.Interface<IOnLoad>().Load());
                            load.AnnotationOrCreate<NXObjectAnnotation>().Load = false;
                        }
                }
            }
            finally
            {
                count--;
            }
        }

        void DeferredInvoke()
        {
            try
            {
                count++;

                bool run;
                do
                {
                    var invokes = host().Xml
                        .DescendantNodesAndSelf()
                        .Where(i => i.NodeType == XmlNodeType.Document || i.NodeType == XmlNodeType.Element)
                        .SelectMany(i => i.Interfaces<IOnInvoke>())
                        .ToLinkedList();

                    run = false;
                    foreach (var invoke in invokes)
                        run |= invoker.Value.Invoke(() => invoke.Invoke());
                }
                while (run);
            }
            finally
            {
                count--;
            }
        }

    }

}
