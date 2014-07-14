using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.DOMEvents;
using NXKit.Xml;

namespace NXKit.XMLEvents
{

    /// <summary>
    /// Listens for a given event on an element.
    /// </summary>
    [Extension]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class ElementEventListener :
        ElementExtension,
        IOnInit
    {

        readonly IInvoker invoker;
        readonly EventListenerAttributes attributes;
        readonly Lazy<IEventHandler> handler;
        readonly Lazy<EventTarget> observer;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="invoker"></param>
        public ElementEventListener(XElement element, IInvoker invoker)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(invoker != null);

            this.invoker = invoker;
            this.attributes = new EventListenerAttributes(element);
            this.handler = new Lazy<IEventHandler>(() => GetHandler());
            this.observer = new Lazy<EventTarget>(() => GetObserver());
        }

        /// <summary>
        /// Gets the handler element.
        /// </summary>
        /// <returns></returns>
        XElement GetHandlerElement()
        {
            if (attributes.Handler != null)
                return Element.ResolveId(attributes.Handler);
            else if (attributes.Observer != null)
                return Element;
            else if (attributes.Observer == null)
                return Element;
            else
                throw new DOMTargetEventException(Element, Events.Error);
        }

        /// <summary>
        /// Gets the handler interface.
        /// </summary>
        /// <returns></returns>
        IEventHandler GetHandler()
        {
            var element = GetHandlerElement();
            if (element != null)
                return element.InterfaceOrDefault<IEventHandler>();

            return null;
        }

        /// <summary>
        /// Gets the observer element.
        /// </summary>
        /// <returns></returns>
        XElement GetObserverElement()
        {
            if (attributes.Observer != null)
                return Element.ResolveId(attributes.Observer);
            else if (attributes.Handler != null)
                return Element;
            else if (attributes.Handler == null)
                return (XElement)Element.Parent;
            else
                throw new DOMTargetEventException(Element, Events.Error);
        }

        /// <summary>
        /// Gets the observer interface.
        /// </summary>
        /// <returns></returns>
        EventTarget GetObserver()
        {
            var element = GetObserverElement();
            if (element != null)
                return element.InterfaceOrDefault<EventTarget>();

            return null;
        }

        /// <summary>
        /// Gets the target element.
        /// </summary>
        /// <returns></returns>
        XElement GetTargetElement()
        {
            if (attributes.Target != null)
                return Element.ResolveId(attributes.Target);

            return null;
        }

        /// <summary>
        /// Gets whether or not the phase is set to 'capture'.
        /// </summary>
        /// <returns></returns>
        public bool GetCapture()
        {
            return attributes.Phase == "capture";
        }

        /// <summary>
        /// Gets whether or not we should propagate.
        /// </summary>
        /// <returns></returns>
        public bool GetPropagate()
        {
            return attributes.Propagate != "stop";
        }

        /// <summary>
        /// Gets whether the default action should execute.
        /// </summary>
        /// <returns></returns>
        public bool InvokeDefaultAction()
        {
            return attributes.DefaultAction != "cancel";
        }

        /// <summary>
        /// Attaches event listeners to elements that possess them.
        /// </summary>
        void Attach()
        {
            var evt = attributes.Event;
            if (evt == null)
                return;

            if (handler.Value == null)
                throw new DOMTargetEventException(Element, Events.Error);

            if (observer.Value == null)
                throw new DOMTargetEventException(Element, Events.Error);

            observer.Value.Register(
                evt,
                InterfaceEventListener.Create(InvokeHandleEvent),
                GetCapture());
        }

        public void InvokeHandleEvent(Event evt)
        {
            Contract.Requires<ArgumentNullException>(evt != null);

            invoker.Invoke(() => 
                handler.Value.HandleEvent(evt));
        }

        void IOnInit.Init()
        {
            Attach();
        }

    }

}
