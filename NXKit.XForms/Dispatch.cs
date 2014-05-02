using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.DOMEvents;
using NXKit.Xml;
using NXKit.XMLEvents;

namespace NXKit.XForms
{

    [Interface("{http://www.w3.org/2002/xforms}dispatch")]
    public class Dispatch :
        ElementExtension,
        IEventHandler
    {

        readonly DispatchAttributes attributes;
        readonly Lazy<EvaluationContextResolver> context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Dispatch(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.attributes = new DispatchAttributes(element);
            this.context = new Lazy<EvaluationContextResolver>(() => element.Interface<EvaluationContextResolver>());
        }

        public void HandleEvent(Event ev)
        {
            Invoke();
        }

        /// <summary>
        /// Gets the targeted element.
        /// </summary>
        /// <returns></returns>
        XElement GetTarget()
        {
            if (attributes.TargetId != null)
                return Element.ResolveId(attributes.TargetId);
            else
                return null;
        }

        Event CreateEvent(string name, bool canBubble, bool cancelable)
        {
            var nxDocumentEvent = Element.Document.Interface<INXDocumentEvent>();
            if (nxDocumentEvent == null)
                throw new InvalidOperationException();

            var documentEvent = Element.Document.Interface<IDocumentEvent>();
            if (documentEvent == null)
                throw new InvalidOperationException();

            var evt = nxDocumentEvent.CreateEvent(name) ?? documentEvent.CreateEvent("Event");
            if (evt != null)
                evt.InitEvent(name, canBubble, cancelable);

            return evt;
        }

        Event GetEvent()
        {
            var name = attributes.Name;
            if (name != null)
                return CreateEvent(name, attributes.Bubbles == "true", attributes.Cancelable == "true");

            return null;
        }

        public void Invoke()
        {
            var target = GetTarget();
            if (target == null)
                return;

            var evt = GetEvent();
            if (evt == null)
                return;

            target.DispatchEvent(evt);
        }

    }

}
