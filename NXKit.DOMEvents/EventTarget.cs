using System;
using System.Collections.Generic;
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

            public void HandleEvent(IEvent evt)
            {
                if (handler != null)
                    handler(evt);
            }

        }

        readonly Visual visual;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="visual"></param>
        public EventTarget(Visual visual)
        {
            this.visual = visual;
        }

        public void AddEventListener(string type, IEventListener listener, bool useCapture)
        {
            // initialize listener map
            var listenerMap = visual.Storage.OfType<EventListenerMap>().FirstOrDefault();
            if (listenerMap == null)
                visual.Storage.AddLast(listenerMap = new EventListenerMap());

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
            var listenerMap = visual.Storage.OfType<EventListenerMap>().FirstOrDefault();
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

        public void DispatchEvent(IEvent @event)
        {
            var evt = @event as Event;
            if (evt == null)
                return;

            var target = visual.Interfaces.OfType<IEventTarget>().FirstOrDefault();
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
            var path = visual.Ascendants().ToList();

            // capture phase
            evt.EventPhase = EventPhase.Capturing;
            foreach (var visual_ in path.Reverse<Visual>())
            {
                HandleEventOnVisual(visual_, evt, true);

                // was told to stop propagation
                if (evt.StopPropagationSet)
                    return;
            }

            // at-target phase
            evt.EventPhase = EventPhase.AtTarget;
            HandleEventOnVisual(visual, evt, false);

            // was told to stop propagation
            if (evt.StopPropagationSet)
                return;

            // bubbling phase
            evt.EventPhase = EventPhase.Bubbling;
            foreach (var visual_ in path)
            {
                HandleEventOnVisual(visual_, evt, false);

                // was told to stop propagation
                if (evt.StopPropagationSet)
                    return;
            }

            if (!evt.PreventDefaultSet)
            {
                // handle default action
                var da = visual as IEventDefaultActionHandler;
                if (da != null)
                    da.DefaultAction(evt);
            }
        }

        /// <summary>
        /// Attempts to handle the event at the given <see cref="Visual"/>.
        /// </summary>
        /// <param name="visual"></param>
        /// <param name="evt"></param>
        /// <param name="useCapture"></param>
        void HandleEventOnVisual(Visual visual, Event evt, bool useCapture)
        {
            evt.CurrentTarget = visual.Interfaces.OfType<IEventTarget>().First();

            var listenerMap = visual.Storage.OfType<EventListenerMap>().FirstOrDefault();
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
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="useCapture"></param>
        /// <param name="handler"></param>
        public void AddEventHandler<T>(string type, bool useCapture, EventHandlerDelegate handler)
            where T : IEvent
        {
            AddEventListener(type, new DelegateDispatchEventListener(handler), useCapture);
        }

    }

}
