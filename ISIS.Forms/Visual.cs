using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

using XForms.Util;
using ISIS.Forms.Events;

namespace ISIS.Forms
{

    /// <summary>
    /// Represents a node in the visual tree.
    /// </summary>
    public abstract class Visual : IEventTarget
    {

        /// <summary>
        /// Manages the lifetime of the currently in scope <see cref="Visual"/>.
        /// </summary>
        public sealed class ThreadScope : IDisposable
        {

            internal ThreadScope(Visual visual)
            {
                Visual.current = visual;
            }


            public void Dispose()
            {
                Visual.current = null;
            }
        }

        [ThreadStatic]
        private static Visual current;

        /// <summary>
        /// Gets the currently in-scope visual.
        /// </summary>
        public static Visual Current
        {
            get { return current; }
        }

        private bool addedEventRaised = false;
        private EventListenerMap listenerMap;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="form"></param>
        /// <param name="parent"></param>
        /// <param name="node"></param>
        protected Visual(IFormProcessor form, StructuralVisual parent, XNode node)
        {
            Form = form;
            Parent = parent;
            Node = node;
            Annotations = new VisualAnnotationCollection();
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="node"></param>
        public Visual(StructuralVisual parent, XNode node)
            : this(parent.Form, parent, node)
        {

        }

        /// <summary>
        /// <see cref="IFormProcessor"/> responsible for the visual tree this visual resides within.
        /// </summary>
        public IFormProcessor Form { get; private set; }

        /// <summary>
        /// Parent <see cref="Visual"/>.
        /// </summary>
        public StructuralVisual Parent { get; private set; }

        /// <summary>
        /// Gets a reference to the underlying DOM node.
        /// </summary>
        public XNode Node { get; private set; }

        /// <summary>
        /// Additional information attached to this <see cref="Visual"/>.
        /// </summary>
        public VisualAnnotationCollection Annotations { get; private set; }

        #region Navigation

        /// <summary>
        /// Yields each ascendant visual.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StructuralVisual> Ascendants()
        {
            var node = this.Parent;
            while (node != null)
            {
                yield return node;
                node = node.Parent;
            }
        }

        #endregion

        #region Naming Scope

        /// <summary>
        /// Finds the <see cref="Visual"/> with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Visual ResolveId(string id)
        {
            // discover the root visual
            var rootVisual = Ascendants()
                .Prepend(this as StructuralVisual)
                .First(i => i != null && i.Parent == null);

            // naming scope of current visual
            var namingScopes = Ascendants()
                .OfType<INamingScope>()
                .Append(null)
                .ToArray();

            // search all descendents of the root element that are sharing naming scopes with myself
            foreach (var visual in rootVisual.DescendantsIncludeNS(namingScopes).OfType<StructuralVisual>())
                if (visual.Id == id)
                    return visual;

            return null;
        }

        /// <summary>
        /// Gets the naming scope of <paramref name="visual"/>.
        /// </summary>
        /// <param name="visual"></param>
        /// <returns></returns>
        protected INamingScope GetNamingScope(Visual visual)
        {
            return visual.Ascendants().OfType<INamingScope>().Append(null).First();
        }

        #endregion

        #region Events

        /// <summary>
        /// Private <see cref="org.w3c.dom.events.EventListener"/> implementation for implementing dispatch to a delegate.
        /// </summary>
        public class DelegateDispatchEventListener : IEventListener
        {

            private FormEventHandler handler;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="observer"></param>
            /// <param name="target"></param>
            /// <param name="handler2"></param>
            public DelegateDispatchEventListener(FormEventHandler handler2)
            {
                this.handler = handler2;
            }

            public void HandleEvent(Event evt)
            {
                if (handler != null)
                    handler(evt);
            }

        }

        /// <summary>
        /// Adds an event handler for events of type <typeparamref name="T"/> when they pass this visual.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        /// <param name="useCapture"></param>
        public void AddEventHandler<T>(FormEventHandler handler, bool useCapture)
        {
            string name = null;

            var nameField = typeof(T).GetField("Name", BindingFlags.Static | BindingFlags.Public);
            if (nameField != null)
                name = (string)nameField.GetValue(null);

            if (name == null)
                throw new Exception();

            AddEventListener(name, new DelegateDispatchEventListener(handler), useCapture);
        }

        /// <summary>
        /// Invoke once per control in refresh to raise the created event.
        /// </summary>
        internal void RaiseAddedEvent()
        {
            if (!addedEventRaised)
            {
                addedEventRaised = true;
                DispatchEvent<VisualAddedEvent>();
            }
        }

        #endregion

        /// <summary>
        /// Returns a string representation of this <see cref="Visual"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return GetType().Name;
        }


        public void AddEventListener(string type, IEventListener listener, bool useCapture)
        {
            // initialize listener map
            if (listenerMap == null)
                listenerMap = new EventListenerMap();

            // initialize listeners list
            var listeners = listenerMap.ValueOrDefault(type);
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
            if (listenerMap == null)
                return;

            // initialize listeners list
            var listeners = listenerMap.ValueOrDefault(type);
            if (listeners == null)
                return;

            // check for existing registration
            var data = listeners.FirstOrDefault(i => i.Listener == listener && i.UseCapture == useCapture);
            if (data != null)
                listeners.Remove(data);
        }

        /// <summary>
        /// Attempts to handle the <see cref="Event"/> with any listeners registered on <paramref name="visual"/>.
        /// </summary>
        /// <param name="visual"></param>
        /// <param name="evt"></param>
        /// <param name="useCapture"></param>
        private void HandleEventOnVisual(Visual visual, Event evt, bool useCapture)
        {
            evt.CurrentTarget = visual;
            if (visual.listenerMap != null)
            {
                // obtain set of registered listeners
                var listeners = visual.listenerMap.ValueOrDefault(evt.Type);
                if (listeners != null)
                    foreach (var listener in listeners.Where(i => i.UseCapture == useCapture))
                        listener.Listener.HandleEvent(evt);
            }
        }

        /// <summary>
        /// Attempts to invoke the default action associated with the event.
        /// </summary>
        /// <param name="visual"></param>
        /// <param name="evt"></param>
        private void HandleDefaultAction(Event evt)
        {
            if (evt.PreventDefaultSet)
                return;

            // type of interface visual needs to implement
            var interfaceType = typeof(IEventDefaultActionHandler<>).MakeGenericType(evt.GetType());

            // if target provides default action, invoke it
            if (interfaceType.IsAssignableFrom(evt.Target.GetType()))
                interfaceType.GetMethod("DefaultAction").Invoke(evt.Target, new object[] { evt });
        }

        /// <summary>
        /// Dispatches a new event of type <typeparamref name="T"/> to this <see cref="Visual"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public void DispatchEvent<T>()
            where T : Event, new()
        {
            DispatchEvent(new T());
        }

        /// <summary>
        /// Dispatches the specified <see cref="Event"/> to this <see cref="Visual"/>.
        /// </summary>
        /// <param name="evt"></param>
        /// <returns></returns>
        public void DispatchEvent(Event evt)
        {
            if (((IEventTarget)this).DispatchEvent(evt))
                HandleDefaultAction(evt);
        }

        /// <summary>
        /// Implements <see cref="IEventTarget"/>.DispatchEvent.
        /// </summary>
        /// <param name="evt"></param>
        /// <returns></returns>
        bool IEventTarget.DispatchEvent(Event evt)
        {
            // event type must be specified
            if (string.IsNullOrEmpty(evt.Type))
                throw new Exception();

            // event phase must be uninitialized
            if (evt.EventPhase != EventPhase.Uninitialized)
                throw new InvalidOperationException();

            // prevent event for dispatch
            evt.Target = this;

            // path to root from root
            var path = Ascendants().ToList();

            // capture phase
            evt.EventPhase = EventPhase.Capturing;
            foreach (var visual in path.Reverse<Visual>())
            {
                HandleEventOnVisual(visual, evt, true);

                // was told to stop propagation
                if (evt.StopPropagationSet)
                    return !evt.PreventDefaultSet;
            }

            // at-target phase
            evt.EventPhase = EventPhase.AtTarget;
            HandleEventOnVisual(this, evt, false);

            // was told to stop propagation
            if (evt.StopPropagationSet)
                return !evt.PreventDefaultSet;

            // bubbling phase
            evt.EventPhase = EventPhase.Bubbling;
            foreach (var visual in path)
            {
                HandleEventOnVisual(visual, evt, false);

                // was told to stop propagation
                if (evt.StopPropagationSet)
                    return !evt.PreventDefaultSet;
            }

            return !evt.PreventDefaultSet;
        }

        /// <summary>
        /// Puts the <see cref="Visual"/> into thread local scope. Dipose of this instance to remove it.
        /// </summary>
        /// <returns></returns>
        public ThreadScope Scope()
        {
            return new ThreadScope(this);
        }

    }

}
