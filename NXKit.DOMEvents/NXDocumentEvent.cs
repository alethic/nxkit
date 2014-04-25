using System;
using System.Diagnostics.Contracts;
using System.Xml;
using System.Xml.Linq;

using NXKit.Xml;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// NX-specified document event interface.
    /// </summary>
    [Interface(XmlNodeType.Document)]
    public class NXDocumentEvent :
        INXDocumentEvent
    {

        readonly XDocument document;
        readonly IEventFactory factory;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="factory"></param>
        public NXDocumentEvent(XDocument document, IEventFactory factory)
        {
            Contract.Requires<ArgumentNullException>(document != null);
            Contract.Requires<ArgumentNullException>(factory != null);

            this.document = document;
            this.factory = factory;
        }

        public T CreateEvent<T>(string type)
            where T : Event
        {
            return factory.CreateEvent<T>(type);
        }

        public Event CreateEvent(string type)
        {
            return factory.CreateEvent(type);
        }

    }

}
