using System;
using System.Diagnostics.Contracts;
using NXKit.DOMEvents;

namespace NXKit.XmlEvents
{

    /// <summary>
    /// Listens for a given event on an element.
    /// </summary>
    public class ElementEventListener
    {

        readonly NXElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public ElementEventListener(NXElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;

            Initialize();
        }

        string GetEvent()
        {
            return (string)element.Attribute(SchemaConstants.Events_1_0 + "event");
        }

        string GetHandlerAttribute()
        {
            return (string)element.Attribute(SchemaConstants.Events_1_0 + "handler");
        }

        string GetObserverAttribute()
        {
            return (string)element.Attribute(SchemaConstants.Events_1_0 + "observer");
        }

        NXElement GetHandlerElement()
        {
            var handlerAttr = GetHandlerAttribute();
            var observerAttr = GetObserverAttribute();

            if (handlerAttr != null)
                return element.ResolveId(handlerAttr);
            else if (observerAttr != null)
                return element;
            else if (observerAttr == null)
                return element;
            else
                throw new InvalidOperationException();
        }

        IEventHandler GetHandler()
        {
            var element = GetHandlerElement();
            if (element != null)
                return element.Interface<IEventHandler>();

            return null;
        }

        NXElement GetObserverElement()
        {
            var handlerAttr = GetHandlerAttribute();
            var observerAttr = GetObserverAttribute();

            if (observerAttr != null)
                return element.ResolveId(observerAttr);
            else if (handlerAttr != null)
                return element;
            else if (handlerAttr == null)
                return (NXElement)element.Parent;
            else
                throw new InvalidOperationException();
        }

        IEventTarget GetObserver()
        {
            var element = GetObserverElement();
            if (element != null)
                return element.Interface<IEventTarget>();

            return null;
        }

        NXElement GetTargetElement()
        {
            var targetAttr = (string)element.Attribute(SchemaConstants.Events_1_0 + "target");
            if (targetAttr != null)
                return element.ResolveId(targetAttr);

            return null;
        }

        public bool GetCapture()
        {
            return (string)element.Attribute(SchemaConstants.Events_1_0 + "phase") == "capture";
        }

        public bool GetPropagate()
        {
            return (string)element.Attribute(SchemaConstants.Events_1_0 + "propagate") != "stop";
        }

        public bool GetDefaultAction()
        {
            return (string)element.Attribute(SchemaConstants.Events_1_0 + "defaultAction") != "cancel";
        }

        void Initialize()
        {
            var evt = GetEvent();
            if (evt == null)
                return;

            var handler = GetHandler();
            if (handler == null)
                throw new InvalidOperationException();

            var observer = GetObserver();
            if (observer == null)
                throw new InvalidOperationException();

            if (handler != null)
                observer.AddEventListener(
                    evt,
                    new EventListener(_ => handler.Handle(_)),
                    GetCapture());
        }

    }

}
