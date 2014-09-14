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
        readonly Extension<EventListenerAttributes> attributes;
        readonly Lazy<IEventHandler> handler;
        readonly Lazy<EventTarget> observer;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributes"></param>
        /// <param name="invoker"></param>
        [ImportingConstructor]
        public ElementEventListener(
            XElement element,
            Extension<EventListenerAttributes> attributes,
            IInvoker invoker)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(attributes != null);
            Contract.Requires<ArgumentNullException>(invoker != null);

            this.invoker = invoker;
            this.attributes = attributes;
            this.handler = new Lazy<IEventHandler>(() => GetHandler());
            this.observer = new Lazy<EventTarget>(() => GetObserver());
        }

        EventListenerAttributes Attributes
        {
            get { return attributes.Value; }
        }

        /// <summary>
        /// Gets the handler element.
        /// </summary>
        /// <returns></returns>
        XElement GetHandlerElement()
        {
            if (Attributes.Handler != null)
                return Element.ResolveId(Attributes.Handler);
            else if (Attributes.Observer != null)
                return Element;
            else if (Attributes.Observer == null)
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
            if (Attributes.Observer != null)
                return Element.ResolveId(Attributes.Observer);
            else if (Attributes.Handler != null)
                return Element;
            else if (Attributes.Handler == null)
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
            if (Attributes.Target != null)
                return Element.ResolveId(Attributes.Target);

            return null;
        }

        /// <summary>
        /// Gets whether or not the phase is set to 'capture'.
        /// </summary>
        /// <returns></returns>
        public bool GetCapture()
        {
            return Attributes.Phase == "capture";
        }

        /// <summary>
        /// Gets whether or not we should propagate.
        /// </summary>
        /// <returns></returns>
        public bool GetPropagate()
        {
            return Attributes.Propagate != "stop";
        }

        /// <summary>
        /// Gets whether the default action should execute.
        /// </summary>
        /// <returns></returns>
        public bool InvokeDefaultAction()
        {
            return Attributes.DefaultAction != "cancel";
        }

        /// <summary>
        /// Attaches event listeners to elements that possess them.
        /// </summary>
        void Attach()
        {
            var evt = Attributes.Event;
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
