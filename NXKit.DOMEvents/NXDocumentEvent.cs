using System;
using System.Diagnostics.Contracts;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// NX-specified document event interface.
    /// </summary>
    public class NXDocumentEvent :
        INXDocumentEvent
    {

        readonly NXNode node;
        readonly IEventFactory factory;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="node"></param>
        public NXDocumentEvent(NXNode node)
        {
            Contract.Requires<ArgumentNullException>(node != null);

            this.node = node;
            this.factory = node.Document.Container.GetExportedValue<IEventFactory>();
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
