using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using NXKit.Util;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// Implements the <see cref="IEventTarget"/> interface.
    /// </summary>
    public class EventTarget :
        IEventTarget
    {

        /// <summary>
        /// Private <see cref="org.w3c.dom.events.EventListener"/> implementation for implementing dispatch to a delegate.
        /// </summary>
        public class DelegateDispatchEventListener :
            IEventListener
        {

            readonly EventHandlerDelegate handler;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="handler"></param>
            public DelegateDispatchEventListener(EventHandlerDelegate handler)
            {
                this.handler = handler;
            }

            public void HandleEvent(Event evt)
            {
                if (handler != null)
                    handler(evt);
            }

        }

        readonly NXElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public EventTarget(NXElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
        }

        public NXElement Node
        {
            get { return element; }
        }

        public void AddEventListener(string type, IEventListener listener, bool useCapture)
        {
            // initialize listener map
            var listenerMap = element.Storage.OfType<EventListenerMap>().FirstOrDefault();
            if (listenerMap == null)
                element.Storage.AddLast(listenerMap = new EventListenerMap());

            // initialize listeners list
            var listeners = listenerMap.GetOrDefault(type);
            if (listeners == null)
                listeners = listenerMap[type] = new List<EventListenerData>();

            // check for existing registration
            if (listeners.Any(i => i.Listener == listener && i.UseCapture == useCapture))
                return;

            // add listener to set
            listeners.Add(new EventListenerData(listener, useCapture));
        }

        public void RemoveEventListener(string type, IEventListener listener, bool useCapture)
        {
            var listenerMap = element.Storage.OfType<EventListenerMap>().FirstOrDefault();
            if (listenerMap == null)
                return;

            // initialize listeners list
            var listeners = listenerMap.GetOrDefault(type);
            if (listeners == null)
                return;

            // check for existing registration
            var data = listeners.FirstOrDefault(i => i.Listener == listener && i.UseCapture == useCapture);
            if (data != null)
                listeners.Remove(data);
        }

        public void DispatchEvent(Event evt)
        {
            var target = element.Interface<IEventTarget>();
            if (target == null)
                throw new Exception();

            // event type must be specified
            if (string.IsNullOrEmpty(evt.Type))
                throw new Exception();

            // event phase must be uninitialized
            if (evt.EventPhase != EventPhase.Uninitialized)
                throw new InvalidOperationException();

            // prevent event for dispatch
            evt.Target = target;

            // path to root from root
            var path = element.Ancestors()
                .OfType<NXElement>()
                .ToList();

            // capture phase
            evt.EventPhase = EventPhase.Capturing;
            foreach (var visual_ in path.Reverse<NXElement>())
            {
                HandleEventOnNode(visual_, evt, true);

                // was told to stop propagation
                if (evt.StopPropagationSet)
                    return;
            }

            // at-target phase
            evt.EventPhase = EventPhase.AtTarget;
            HandleEventOnNode(element, evt, false);

            // was told to stop propagation
            if (evt.StopPropagationSet)
                return;

            // bubbling phase
            evt.EventPhase = EventPhase.Bubbling;
            foreach (var visual_ in path)
            {
                HandleEventOnNode(visual_, evt, false);

                // was told to stop propagation
                if (evt.StopPropagationSet)
                    return;
            }

            if (!evt.PreventDefaultSet)
            {
                // handle default action
                var da = element as IEventDefaultActionHandler;
                if (da != null)
                    da.DefaultAction(evt);
            }
        }

        /// <summary>
        /// Attempts to handle the event at the given <see cref="NXNode"/>.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="evt"></param>
        /// <param name="useCapture"></param>
        void HandleEventOnNode(NXElement node, Event evt, bool useCapture)
        {
            evt.CurrentTarget = node.Interface<IEventTarget>();

            var listenerMap = node.Storage.OfType<EventListenerMap>().FirstOrDefault();
            if (listenerMap != null)
            {
                // obtain set of registered listeners
                var listeners = listenerMap.GetOrDefault(evt.Type);
                if (listeners != null)
                    foreach (var listener in listeners.Where(i => i.UseCapture == useCapture))
                        listener.Listener.HandleEvent(evt);
            }
        }

        /// <summary>
        /// Adds an event handler for the given event type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="useCapture"></param>
        /// <param name="handler"></param>
        public void AddEventHandler(string type, bool useCapture, EventHandlerDelegate handler)
        {
            AddEventListener(type, new DelegateDispatchEventListener(handler), useCapture);
        }

    }

}
