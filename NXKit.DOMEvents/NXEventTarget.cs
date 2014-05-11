using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.Xml;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// Implements the <see cref="INXEventTarget"/> interface.
    /// </summary>
    [Interface(XmlNodeType.Document)]
    [Interface(XmlNodeType.Element)]
    [Interface(XmlNodeType.Text)]
    public class NXEventTarget :
        INXEventTarget
    {

        readonly XNode node;
        readonly IEventFactory events;
        readonly Lazy<IEventTarget> target;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="events"></param>
        [ImportingConstructor]
        public NXEventTarget(XNode element, IEventFactory events)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(events != null);

            this.node = element;
            this.events = events;
            this.target = new Lazy<IEventTarget>(() => element.Interface<IEventTarget>());
        }

        public XNode Node
        {
            get { return node; }
        }

        public IEventTarget Target
        {
            get { return target.Value; }
        }

        public Event DispatchEvent(string type)
        {
            var evt = events.CreateEvent(type);
            if (evt == null)
                throw new NullReferenceException();

            Target.DispatchEvent(evt);

            return evt;
        }

        public Event DispatchEvent(string type, object context)
        {
            var evt = events.CreateEvent(type);
            if (evt == null)
                throw new NullReferenceException();

            // set supplied context information
            evt.Context = context;

            Target.DispatchEvent(evt);

            return evt;
        }

        public void AddEventHandler(string type, EventHandlerDelegate handler)
        {
            AddEventHandler(type, false, handler);
        }

        public void AddEventHandler(string type, bool useCapture, EventHandlerDelegate handler)
        {
            Target.AddEventListener(type, new ActionEventListener(_ => handler(_)), useCapture);
        }

    }

}
