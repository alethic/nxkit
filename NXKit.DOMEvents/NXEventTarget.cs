using System;
using System.Diagnostics.Contracts;
using System.Xml;
using System.Xml.Linq;

using NXKit.Xml;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// Implements the <see cref="INXEventTarget"/> interface.
    /// </summary>
    [Interface(XmlNodeType.Element)]
    public class NXEventTarget :
        INXEventTarget
    {

        readonly XElement element;
        readonly IEventFactory provider;
        readonly Lazy<IEventTarget> target;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public NXEventTarget(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Ensures(provider != null);

            this.element = element;
            this.provider = element.Host().Container.GetExportedValue<IEventFactory>();
            this.target = new Lazy<IEventTarget>(() => element.Interface<IEventTarget>());
        }

        public XElement Element
        {
            get { return element; }
        }

        public IEventTarget Target
        {
            get { return target.Value; }
        }

        public void DispatchEvent(string type)
        {
            var evt = provider.CreateEvent(type);
            if (evt == null)
                throw new NullReferenceException();

            Target.DispatchEvent(evt);
        }

        public void AddEventHandler(string type, EventHandlerDelegate handler)
        {
            AddEventHandler(type, false, handler);
        }

        public void AddEventHandler(string type, bool useCapture, EventHandlerDelegate handler)
        {
            Target.AddEventListener(type, new EventListener(_ => handler(_)), useCapture);
        }

    }

}
