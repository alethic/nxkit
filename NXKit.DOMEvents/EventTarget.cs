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
        public EventTarget(
            ITraceService trace,
            XNode node)
        {
            Contract.Requires<ArgumentNullException>(trace != null);
            Contract.Requires<ArgumentNullException>(node != null);

            this.trace = trace;
            this.node = node;
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
            var path = node.Ancestors()
                .Cast<XNode>()
                .Append(node.Document)
                .ToList();

            // capture phase
            evt.EventPhase = EventPhase.Capturing;
            foreach (var visual_ in path.Reverse<XNode>())
            {
                HandleEventOnNode(visual_, evt, true);

                // was told to stop propagation
                if (evt.StopPropagationSet)
                    return;
            }

            // at-target phase
            evt.EventPhase = EventPhase.AtTarget;
            HandleEventOnNode(node, evt, false);

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
                foreach (var da in node.Interfaces<IEventDefaultActionHandler>())
                    if (da != null)
                        da.DefaultAction(evt);
            }
        }

        /// <summary>
        /// Attempts to handle the event at the given <see cref="XElement"/>.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="evt"></param>
        /// <param name="useCapture"></param>
        void HandleEventOnNode(XNode node, Event evt, bool useCapture)
        {
            Contract.Requires<ArgumentNullException>(node != null);
            Contract.Requires<ArgumentNullException>(evt != null);

            evt.CurrentTarget = node.Interface<IEventTarget>();

            var items = node.AnnotationOrCreate<EventTargetState>().Listeners
                .Where(i => i.EventType == evt.Type)
                .Where(i => i.UseCapture == useCapture);

            foreach (var listener in items)
                listener.Listener.HandleEvent(evt);
        }

    }

}
