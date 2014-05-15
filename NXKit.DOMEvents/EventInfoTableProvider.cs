using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;

using NXKit.Composition;
using NXKit.Xml;

namespace NXKit.DOMEvents
{

    [ScopeExport(typeof(IEventProvider), Scope.Host)]
    public class EventInfoTableProvider :
        IEventProvider
    {

        readonly IEnumerable<IEventInfoTable> tables;
        readonly Lazy<IDocumentEvent> documentEvent;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="tables"></param>
        [ImportingConstructor]
        public EventInfoTableProvider(
            Func<NXDocumentHost> host,
            [ImportMany] IEnumerable<IEventInfoTable> tables)
        {
            Contract.Requires<ArgumentNullException>(host != null);
            Contract.Requires<ArgumentNullException>(tables != null);

            this.tables = tables;
            this.documentEvent = new Lazy<IDocumentEvent>(() => host().Xml.Interface<IDocumentEvent>());
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
