using System;
using System.Diagnostics.Contracts;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// Implements the <see cref="INXEventTarget"/> interface.
    /// </summary>
    public class NXEventTarget :
        INXEventTarget
    {

        readonly NXElement element;
        readonly IEventFactory provider;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public NXEventTarget(NXElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Ensures(provider != null);

            this.element = element;
            this.provider = element.Document.Container.GetExportedValue<IEventFactory>();
        }

        public NXElement Elemenet
        {
            get { return element; }
        }

        public IEventTarget Target
        {
            get { Contract.Ensures(Contract.Result<IEventTarget>() != null); return Elemenet.Interface<IEventTarget>(); }
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
