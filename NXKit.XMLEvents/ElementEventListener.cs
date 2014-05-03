using System;
using System.Diagnostics.Contracts;
using System.Xml;
using System.Xml.Linq;

using NXKit.DOMEvents;
using NXKit.Xml;

namespace NXKit.XMLEvents
{

    /// <summary>
    /// Listens for a given event on an element.
    /// </summary>
    [Interface(XmlNodeType.Element)]
    public class ElementEventListener :
        IOnLoad
    {

        readonly NXDocumentHost host;
        readonly XElement element;
        readonly EventListenerAttributes attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="element"></param>
        public ElementEventListener(NXDocumentHost host, XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
            this.attributes = new EventListenerAttributes(element);
        }

        /// <summary>
        /// Gets the handler element.
        /// </summary>
        /// <returns></returns>
        XElement GetHandlerElement()
        {
            if (attributes.Handler != null)
                return element.ResolveId(attributes.Handler);
            else if (attributes.Observer != null)
                return element;
            else if (attributes.Observer == null)
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
            if (attributes.Observer != null)
                return element.ResolveId(attributes.Observer);
            else if (attributes.Handler != null)
                return element;
            else if (attributes.Handler == null)
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
            if (attributes.Target != null)
                return element.ResolveId(attributes.Target);

            return null;
        }

        /// <summary>
        /// Gets whether or not the phase is set to 'capture'.
        /// </summary>
        /// <returns></returns>
        public bool GetCapture()
        {
            return attributes.Phase == "capture";
        }

        /// <summary>
        /// Gets whether or not we should propagate.
        /// </summary>
        /// <returns></returns>
        public bool GetPropagate()
        {
            return attributes.Propagate != "stop";
        }

        /// <summary>
        /// Gets whether the default action should execute.
        /// </summary>
        /// <returns></returns>
        public bool InvokeDefaultAction()
        {
            return attributes.DefaultAction != "cancel";
        }

        /// <summary>
        /// Attaches event listeners to elements that possess them.
        /// </summary>
        void Attach()
        {
            var evt = attributes.Event;
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
                    new EventListener(_ => handler.HandleEvent(_)),
                    GetCapture());
        }

        void IOnLoad.Load()
        {
            Attach();
        }

    }

}
