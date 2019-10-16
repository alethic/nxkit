using System;
using System.Xml.Linq;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// NX-specified document event interface.
    /// </summary>
    [Extension(ExtensionObjectType.Document)]
    public class NXDocumentEvent :
        DocumentExtension,
        INXDocumentEvent
    {

        readonly IEventFactory factory;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="factory"></param>
        public NXDocumentEvent(
            XDocument document,
            IEventFactory factory)
            : base(document)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
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
