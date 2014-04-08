using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit;
using NXKit.DOMEvents;

namespace NXKit.XmlEvents
{

    /// <summary>
    /// Listens for a given event on an element.
    /// </summary>
    public class ElementEventListener
    {

        readonly XElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public ElementEventListener(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;

            Initialize();
        }

        /// <summary>
        /// Gets the value of the 'event' attribute.
        /// </summary>
        /// <returns></returns>
        string GetEvent()
        {
            return (string)element.Attribute(SchemaConstants.Events_1_0 + "event");
        }

        /// <summary>
        /// Gets the value of the 'handler' attribute.
        /// </summary>
        /// <returns></returns>
        string GetHandlerAttribute()
        {
            return (string)element.Attribute(SchemaConstants.Events_1_0 + "handler");
        }

        /// <summary>
        /// Gets the value of the 'observer' attribute.
        /// </summary>
        /// <returns></returns>
        string GetObserverAttribute()
        {
            return (string)element.Attribute(SchemaConstants.Events_1_0 + "observer");
        }

        /// <summary>
        /// Gets the handler element.
        /// </summary>
        /// <returns></returns>
        XElement GetHandlerElement()
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

        /// <summary>
        /// Gets the handler interface.
        /// </summary>
        /// <returns></returns>
        IEventHandler GetHandler()
        {
            var element = GetHandlerElement();
            if (element != null)
                return element.Interface<IEventHandler>();

            return null;
        }

        /// <summary>
        /// Gets the observer element.
        /// </summary>
        /// <returns></returns>
        XElement GetObserverElement()
        {
            var handlerAttr = GetHandlerAttribute();
            var observerAttr = GetObserverAttribute();

            if (observerAttr != null)
                return element.ResolveId(observerAttr);
            else if (handlerAttr != null)
                return element;
            else if (handlerAttr == null)
                return (XElement)element.Parent;
            else
                throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets the observer interface.
        /// </summary>
        /// <returns></returns>
        IEventTarget GetObserver()
        {
            var element = GetObserverElement();
            if (element != null)
                return element.Interface<IEventTarget>();

            return null;
        }

        /// <summary>
        /// Gets the target element.
        /// </summary>
        /// <returns></returns>
        XElement GetTargetElement()
        {
            var targetAttr = (string)element.Attribute(SchemaConstants.Events_1_0 + "target");
            if (targetAttr != null)
                return element.ResolveId(targetAttr);

            return null;
        }

        /// <summary>
        /// Gets whether or not the phase is set to 'capture'.
        /// </summary>
        /// <returns></returns>
        public bool GetCapture()
        {
            return (string)element.Attribute(SchemaConstants.Events_1_0 + "phase") == "capture";
        }

        /// <summary>
        /// Gets whether or not we should propagate.
        /// </summary>
        /// <returns></returns>
        public bool GetPropagate()
        {
            return (string)element.Attribute(SchemaConstants.Events_1_0 + "propagate") != "stop";
        }

        /// <summary>
        /// Gets whether the default action should execute.
        /// </summary>
        /// <returns></returns>
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
