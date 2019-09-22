using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.DOMEvents;
using NXKit.Xml;
using NXKit.XMLEvents;

namespace NXKit.XForms
{

    [Extension("{http://www.w3.org/2002/xforms}dispatch")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class Dispatch :
        ElementExtension,
        IEventHandler
    {

        readonly DispatchAttributes attributes;
        readonly Extension<EvaluationContextResolver> context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributes"></param>
        /// <param name="context"></param>
        [ImportingConstructor]
        public Dispatch(
            XElement element,
            DispatchAttributes attributes,
            Extension<EvaluationContextResolver> context)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            this.attributes = attributes ?? throw new ArgumentNullException(nameof(attributes));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
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
