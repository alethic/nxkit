using System;
using System.Collections.Generic;
using System.Linq;

using NXKit.Composition;
using NXKit.Xml;

namespace NXKit.DOMEvents
{

    [Export(typeof(IEventProvider), CompositionScope.Host)]
    public class EventInfoTableProvider :
        IEventProvider
    {

        readonly IEnumerable<IEventInfoTable> tables;
        readonly Lazy<IDocumentEvent> documentEvent;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="environment"></param>
        /// <param name="tables"></param>
        public EventInfoTableProvider(
            DocumentEnvironment environment,
            IEnumerable<IEventInfoTable> tables)
        {
            if (environment == null)
                throw new ArgumentNullException(nameof(environment));

            this.tables = tables ?? throw new ArgumentNullException(nameof(tables));
            this.documentEvent = new Lazy<IDocumentEvent>(() => environment.GetHost().Xml.Interface<IDocumentEvent>());
        }

        public Event CreateEvent(string type)
        {
            var evt = tables
                .SelectMany(i => i.GetEventInfos())
                .Where(i => i.Type == type)
                .Select(i => new { Event = documentEvent.Value.CreateEvent(i.EventInterface), EventInfo = i })
                .Where(i => i.Event != null)
                .FirstOrDefault();

            // initialize and return event
            if (evt != null)
            {
                evt.EventInfo.InitEvent(evt.Event);
                evt.Event.isTrusted = true;
                return evt.Event;
            }

            return null;
        }

    }

}
