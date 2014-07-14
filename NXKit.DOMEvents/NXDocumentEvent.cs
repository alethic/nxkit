using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
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
