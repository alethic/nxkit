using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// NX-specified document event interface.
    /// </summary>
    public class NXDocumentEvent :
        INXDocumentEvent
    {

        readonly XDocument element;
        IEventFactory factory;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="document"></param>
        public NXDocumentEvent(XDocument document)
        {
            Contract.Requires<ArgumentNullException>(document != null);

            this.element = document;
        }

        IEventFactory Factory
        {
            get { return factory ?? (factory = element.Host().Container.GetExportedValue<IEventFactory>()); }
        }

        public T CreateEvent<T>(string type)
            where T : Event
        {
            return Factory.CreateEvent<T>(type);
        }

        public Event CreateEvent(string type)
        {
            return Factory.CreateEvent(type);
        }

    }

}
