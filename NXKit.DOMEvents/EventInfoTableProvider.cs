using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;

namespace NXKit.DOMEvents
{

    [Export(typeof(IEventProvider))]
    public class EventInfoTableProvider :
        IEventProvider
    {

        readonly NXDocument document;
        readonly IEnumerable<IEventInfoTable> tables;
        IDocumentEvent documentEvent;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="tables"></param>
        [ImportingConstructor]
        public EventInfoTableProvider(
            NXDocument document,
            [ImportMany] IEnumerable<IEventInfoTable> tables)
        {
            Contract.Requires<ArgumentNullException>(document != null);
            Contract.Requires<ArgumentNullException>(tables != null);

            this.document = document;
            this.tables = tables;
        }

        IDocumentEvent DocumentEvent
        {
            get { return documentEvent ?? (documentEvent = document.Interface<IDocumentEvent>()); }
        }

        public Event CreateEvent(string type)
        {
            var evt = tables
                .SelectMany(i => i.GetEventInfos())
                .Where(i => i.Type == type)
                .Select(i => new { Event = DocumentEvent.CreateEvent(i.EventInterface), EventInfo = i })
                .FirstOrDefault();

            // initialize and return event
            if (evt != null)
            {
                evt.EventInfo.InitEvent(evt.Event);
                return evt.Event;
            }

            return null;
        }

    }

}
