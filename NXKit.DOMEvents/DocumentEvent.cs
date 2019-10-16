using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// Provides a <see cref="IDocumentEvent"/> implementation.
    /// </summary>
    [Extension(typeof(IDocumentEvent), ExtensionObjectType.Document)]
    public class DocumentEvent :
        DocumentExtension,
        IDocumentEvent
    {

        readonly IEnumerable<IEventInstanceProvider> providers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="providers"></param>
        public DocumentEvent(
            XDocument document,
            IEnumerable<IEventInstanceProvider> providers)
            : base(document)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            this.providers = providers ?? throw new ArgumentNullException(nameof(providers));
        }

        public Event CreateEvent(string eventInterface)
        {
            return providers
                .Select(i => i.CreateEvent(eventInterface))
                .Where(i => i != null)
                .FirstOrDefault();
        }

    }

}
