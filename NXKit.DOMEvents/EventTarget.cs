using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;
using NXKit.Composition;
using NXKit.Diagnostics;
using NXKit.DOM;
using NXKit.Util;
using NXKit.Xml;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// Manages event listener registrations and event dispatching for a given <see cref="XNode"/>.
    /// </summary>
    [Extension(ExtensionObjectType.Document | ExtensionObjectType.Element | ExtensionObjectType.Text)]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    [Remote]
    public class EventTarget :
        NodeExtension,
        IEventTarget
    {

        readonly IEventFactory events;
        readonly ITraceService trace;
        readonly IInvoker invoker;
        readonly XNode node;
        readonly EventTargetState state;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="events"></param>
        /// <param name="trace"></param>
        /// <param name="invoker"></param>
        [ImportingConstructor]
        public EventTarget(
            XNode node,
            IEventFactory events,
            ITraceService trace,
            IInvoker invoker)
            : base(node)
        {
            Contract.Requires<ArgumentNullException>(node != null);
            Contract.Requires<ArgumentNullException>(events != null);
            Contract.Requires<ArgumentNullException>(trace != null);
            Contract.Requires<ArgumentNullException>(invoker != null);

            this.node = node;
            this.events = events;
            this.trace = trace;
            this.invoker = invoker;
            this.state = node.AnnotationOrCreate<EventTargetState>();
        }

        /// <summary>
        /// Gets the set of <see cref="IEventListener"/>s which are registered on this instance.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EventListenerRegistration> GetRegistrations()
        {
            return state.registrations;
        }

        /// <summary>
        /// Adds the given <see cref="IEventListener"/> to this <see cref="XNode"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="listener"></param>
        /// <param name="capture"></param>
        public void Register(string type, IEventListener listener, bool capture)
        {
            Contract.Requires<ArgumentNullException>(type != null);
            Contract.Requires<ArgumentNullException>(listener != null);

            state.registrations = state.registrations.Add(new EventListenerRegistration(type, listener, capture));
        }

        /// <summary>
        /// Removes the given <see cref="IEventListener"/> from this <see cref="XNode"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="listener"></param>
        /// <param name="capture"></param>
        public void Unregister(string type, IEventListener listener, bool capture)
        {
            Contract.Requires<ArgumentNullException>(type != null);
            Contract.Requires<ArgumentNullException>(listener != null);

            var items = state.registrations
                .Where(i => i.EventType == type)
                .Where(i => i.Capture == capture)
                .Where(i => i.Listener == listener);

            foreach (var item in items)
                state.registrations = state.registrations.Remove(item);
        }

        /// <summary>
        /// Dispatches the event to this <see cref="XNode"/>.
        /// </summary>
        /// <param name="evt"></param>
        /// <returns></returns>
        public bool Dispatch(Event evt)
        {
            Contract.Requires<ArgumentNullException>(evt != null);

            trace.Information("EventDispatcher.DispatchEvent: {0} to {1}", evt.Type, node);

            if (evt.dispatch ||
                evt.initialized == false ||
                evt.type == null)
                throw new DOMException(DOMException.INVALID_STATE_ERR);

            evt.dispatch = true;
            evt.target = node;

            // path to root from root
            var path = node.Ancestors()
                .Cast<XNode>()
                .Append(node.Document)
                .ToLinkedList();

            evt.eventPhase = EventPhase.Capturing;

            // capture phase moves from root to target
            foreach (var currentTarget in path.Backwards())
            {
                if (evt.stopPropagation)
                    break;

                InvokeListeners(currentTarget, evt);
            }

            // at-target phase
            evt.eventPhase = EventPhase.AtTarget;

            if (!evt.stopPropagation)
                InvokeListeners(node, evt);

            // bubbling phase
            if (evt.bubbles)
            {
                evt.eventPhase = EventPhase.Bubbling;

                // bubbling phase moves from target to root
                foreach (var currentTarget in path.Forwards())
                {
                    if (evt.stopPropagation)
                        break;

                    InvokeListeners(currentTarget, evt);
                }
            }

            evt.dispatch = false;
            evt.eventPhase = EventPhase.None;
            evt.currentTarget = null;

            if (evt.canceled)
                return false;

            // handle default action
            foreach (var da in node.Interfaces<IEventDefaultAction>())
                if (da != null)
                    da.DefaultAction(evt);

            return true;
        }

        /// <summary>
        /// Invokes the applicable listeners for this node.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="evt"></param>
        void InvokeListeners(XNode node, Event evt)
        {
            Contract.Requires<ArgumentNullException>(node != null);
            Contract.Requires<ArgumentNullException>(evt != null);

            // Initialize event's currentTarget attribute to the object for which these steps are run.
            evt.currentTarget = node;

            // invoke registered listeners
            foreach (var registration in node.Interface<EventTarget>().state.registrations)
            {
                // If event's stop immediate propagation flag is set, terminate the invoke algorithm.
                if (evt.stopImmediatePropagation)
                    return;

                // If event's type attribute value is not listener's type, terminate these substeps (and run them for
                // the next event listener).
                if (evt.type != registration.EventType)
                    continue;

                // If event's eventPhase attribute value is CAPTURING_PHASE and listener's capture is false, terminate
                // these substeps (and run them for the next event listener).
                if (evt.eventPhase == EventPhase.Capturing && registration.Capture == false)
                    continue;

                // If event's eventPhase attribute value is BUBBLING_PHASE and listener's capture is true, terminate
                // these substeps (and run them for the next event listener).
                if (evt.eventPhase == EventPhase.Bubbling && registration.Capture == true)
                    continue;

                // Call listener's callback's handleEvent, with the event passed to this algorithm as the first
                // argument and event's currentTarget attribute value as callback this value.
                invoker.Invoke(() =>
                    registration.Listener.HandleEvent(evt));
            }

            // invoke listeners available directly as interfaces
            foreach (var listener in node.Interfaces<IEventListener>())
            {
                // If event's stop immediate propagation flag is set, terminate the invoke algorithm.
                if (evt.stopImmediatePropagation)
                    return;

                invoker.Invoke(() =>
                    listener.HandleEvent(evt));
            }
        }

        /// <summary>
        /// Dispatches a trusted event by name.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [Remote]
        public Event Dispatch(string type)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(type));

            var evt = events.CreateEvent(type);
            if (evt == null)
                throw new DOMException();

            // dispatch event internally
            Dispatch(evt);

            return evt;
        }

        /// <summary>
        /// Dispatches a trusted event by name.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Event Dispatch(string type, object context)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(type));

            var evt = events.CreateEvent(type);
            if (evt == null)
                throw new DOMException();

            // set supplied context information
            evt.Context = context;

            // dispatch event internally
            Dispatch(evt);

            return evt;
        }

        /// <summary>
        /// Adds an event handler delegate.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="handler"></param>
        public void AddEventDelegate(string type, EventHandlerDelegate handler)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(type));
            Contract.Requires<ArgumentNullException>(handler != null);

            AddEventDelegate(type, handler, false);
        }

        /// <summary>
        /// Adds an event handler delegate.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="handler"></param>
        /// <param name="capture"></param>
        public void AddEventDelegate(string type, EventHandlerDelegate handler, bool capture)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(type));
            Contract.Requires<ArgumentNullException>(handler != null);

            Register(type, new ActionEventListener(_ => handler(_)), capture);
        }

        void IEventTarget.AddEventListener(string type, IEventListener listener, bool useCapture)
        {
            Register(type, listener, useCapture);
        }

        void IEventTarget.AddEventListener(string type, IEventListener listener)
        {
            Register(type, listener, false);
        }

        void IEventTarget.RemoveEventListener(string type, IEventListener listener, bool useCapture)
        {
            Unregister(type, listener, useCapture);
        }

        void IEventTarget.RemoveEventListener(string type, IEventListener listener)
        {
            Unregister(type, listener, false);
        }

        bool IEventTarget.DispatchEvent(Event evt)
        {
            evt.isTrusted = false;
            return Dispatch(evt);
        }

    }

}
