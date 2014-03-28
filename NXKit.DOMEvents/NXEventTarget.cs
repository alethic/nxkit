﻿using System;
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
        readonly IEventTarget target;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public NXEventTarget(NXElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Ensures(target != null);

            this.element = element;
            this.provider = element.Document.Container.GetExportedValue<IEventFactory>();
            this.target = element.Interface<IEventTarget>();
        }

        public NXElement Node
        {
            get { return element; }
        }

        public void DispatchEvent(string type)
        {
            var evt = provider.CreateEvent(type);
            if (evt == null)
                throw new NullReferenceException();

            target.DispatchEvent(evt);
        }

        public void AddEventHandler(string type, EventHandlerDelegate handler)
        {
            AddEventHandler(type, false, handler);
        }

        public void AddEventHandler(string type, bool useCapture, EventHandlerDelegate handler)
        {
            target.AddEventListener(type, new EventListener(_ => handler(_)), useCapture);
        }

    }

}