using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

using NXKit.Util;
using NXKit.Xml;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// Implements the <see cref="IEventTarget"/> interface.
    /// </summary>
    [Interface(XmlNodeType.Element)]
    [Interface(XmlNodeType.Text)]
    public class EventTarget :
        IEventTarget
    {

        readonly XNode node;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="node"></param>
        public EventTarget(XNode node)
        {
            Contract.Requires<ArgumentNullException>(node != null);

            this.node = node;
        }

        public XNode Node
        {
            get { return node; }
        }

        public void AddEventListener(string type, IEventListener listener, bool useCapture)
        {
            // initialize listener map
            var listenerMap = node.Annotation<EventListenerMap>();
            if (listenerMap == null)
                node.AddAnnotation(listenerMap = new EventListenerMap());

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
            var listenerMap = node.Annotation<EventListenerMap>();
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

            var listenerMap = node.Annotation<EventListenerMap>();
            if (listenerMap != null)
            {
                // obtain set of registered listeners
                var listeners = listenerMap.GetOrDefault(evt.Type);
                if (listeners != null)
                    foreach (var listener in listeners.Where(i => i.UseCapture == useCapture))
                        listener.Listener.HandleEvent(evt);
            }
        }

    }

}
