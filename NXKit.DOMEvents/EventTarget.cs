using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

using NXKit.Diagnostics;
using NXKit.Util;
using NXKit.Xml;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// Implements the <see cref="IEventTarget"/> interface.
    /// </summary>
    [Interface(XmlNodeType.Document)]
    [Interface(XmlNodeType.Element)]
    [Interface(XmlNodeType.Text)]
    public class EventTarget :
        IEventTarget
    {

        readonly ITraceService trace;
        readonly XNode node;
        readonly EventTargetState state;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="node"></param>
        [ImportingConstructor]
        public EventTarget(XNode node, ITraceService trace)
        {
            Contract.Requires<ArgumentNullException>(node != null);
            Contract.Requires<ArgumentNullException>(trace != null);

            this.node = node;
            this.trace = trace;
            this.state = node.AnnotationOrCreate<EventTargetState>();
        }

        public IEnumerable<IEventListener> GetEventListeners(string type, bool useCapture)
        {
            return state.Listeners
                .Where(i => i.EventType == type && i.UseCapture == useCapture)
                .Select(i => i.Listener);
        }

        public bool HasEventListener(string type, IEventListener listener, bool useCapture)
        {
            return GetEventListeners(type, useCapture)
                .Any(i => object.Equals(i, listener));
        }

        public void AddEventListener(string type, IEventListener listener, bool useCapture)
        {
            state.Listeners.Add(new EventTargetListenerItem(type, useCapture, listener));
        }

        public void RemoveEventListener(string type, IEventListener listener, bool useCapture)
        {
            var items = state.Listeners
                .Where(i => i.EventType == type)
                .Where(i => i.UseCapture == useCapture)
                .Where(i => i.Listener == listener);

            foreach (var item in items)
                state.Listeners.Remove(item);
        }

        public void DispatchEvent(Event evt)
        {
            trace.Information("EventTarget.DispatchEvent: {0} to {1}", evt.Type, node);

            var target = node.Interface<IEventTarget>();
            if (target == null)
                throw new InvalidOperationException();

            // event type must be specified
            if (string.IsNullOrEmpty(evt.Type))
                throw new InvalidOperationException();

            // event phase must be uninitialized
            if (evt.EventPhase != EventPhase.Uninitialized)
                throw new InvalidOperationException();

            // prevent event for dispatch
            evt.Target = target;

            // path to root from root
            var path = node.Ancestors()
                .Cast<XNode>()
                .Append(node.Document)
                .ToLinkedList();

            // capture phase
            evt.EventPhase = EventPhase.Capturing;

            // capture phase moves from root to target
            foreach (var n in path.Backwards().Select(i => i.Value))
            {
                HandleEventOnNode(n, evt);

                // was told to stop propagation
                if (evt.StopPropagationSet)
                    return;
            }

            // at-target phase
            evt.EventPhase = EventPhase.AtTarget;
            HandleEventOnNode(node, evt);

            // was told to stop propagation
            if (evt.StopPropagationSet)
                return;

            // bubbling phase
            evt.EventPhase = EventPhase.Bubbling;

            // bubbling phase moves from target to root
            foreach (var n in path)
            {
                HandleEventOnNode(n, evt);

                // was told to stop propagation
                if (evt.StopPropagationSet)
                    return;
            }

            // handle default action
            if (!evt.PreventDefaultSet)
                foreach (var da in node.Interfaces<IEventDefaultActionHandler>())
                    if (da != null)
                        da.DefaultAction(evt);
        }

        /// <summary>
        /// Attempts to handle the event at the given <see cref="XNode"/>.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="evt"></param>
        void HandleEventOnNode(XNode node, Event evt)
        {
            Contract.Requires<ArgumentNullException>(node != null);
            Contract.Requires<ArgumentNullException>(evt != null);

            evt.CurrentTarget = node.Interface<IEventTarget>();

            var listeners = node.AnnotationOrCreate<EventTargetState>().Listeners
                .Where(i => i.EventType == evt.Type)
                .Where(i => i.UseCapture == (evt.EventPhase == EventPhase.Capturing))
                .Select(i => i.Listener);

            foreach (var listener in listeners)
                listener.HandleEvent(evt);
        }

    }

}
