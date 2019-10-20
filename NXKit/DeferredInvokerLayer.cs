using System;
using System.Linq;
using System.Xml;

using NXKit.Composition;
using NXKit.Diagnostics;
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
        readonly ITraceService trace;

        int count = 0;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="invoker"></param>
        /// <param name="trace"></param>
        public DeferredInvokerLayer(Func<Document> host, IExport<IInvoker> invoker, ITraceService trace)
        {
            this.host = host ?? throw new ArgumentNullException(nameof(host));
            this.invoker = invoker ?? throw new ArgumentNullException(nameof(invoker));
            this.trace = trace ?? throw new ArgumentNullException(nameof(trace));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="environment"></param>
        /// <param name="invoker"></param>
        public DeferredInvokerLayer(DocumentEnvironment environment, IExport<IInvoker> invoker, ITraceService trace) :
            this(() => environment?.GetHost(), invoker, trace)
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
            trace.Debug("Processing deferred behaviors.");

            DeferredInit();
            DeferredLoad();
            DeferredInvoke();
        }

        void DeferredInit()
        {

            try
            {
                count++;
                trace.Debug("Processing deferred init behaviors at depth {Count}.", count);

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

                    trace.Debug("Executing {Count} deferred init executions.", inits.Count);
                    foreach (var init in inits)
                    {
                        if (init.Document == null)
                        {
                            trace.Debug("Skipping deferred init, detached node.");
                            continue;
                        }

                        trace.Debug("Executing deferred init for {NodeId}.", init.GetObjectId());
                        foreach (var m in init.Interfaces<IOnInit>())
                            invoker.Value.Invoke(m.Init);
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
                trace.Debug("Processing deferred load behaviors at depth {Count}.", count);

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

                    trace.Debug("Executing {Count} deferred load executions.", loads.Count);
                    foreach (var load in loads)
                    {
                        if (load.Document == null)
                        {
                            trace.Debug("Skipping deferred load, detached node.");
                            continue;
                        }

                        trace.Debug("Executing deferred load for {NodeId}.", load.GetObjectId());
                        foreach (var m in load.Interfaces<IOnLoad>())
                            invoker.Value.Invoke(m.Load);
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
