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
        IEventFactory factory;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="node"></param>
        public NXDocumentEvent(NXNode node)
        {
            Contract.Requires<ArgumentNullException>(node != null);

            this.node = node;
        }

        IEventFactory Factory
        {
            get { return factory ?? (factory = node.Document.Container.GetExportedValue<IEventFactory>()); }
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
