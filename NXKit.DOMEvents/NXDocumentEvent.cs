using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// NX-specified document event interface.
    /// </summary>
    [Extension(ExtensionObjectType.Document)]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
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
        [ImportingConstructor]
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
