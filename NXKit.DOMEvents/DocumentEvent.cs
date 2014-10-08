using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// Provides a <see cref="IDocumentEvent"/> implementation.
    /// </summary>
    [Extension(ExtensionObjectType.Document)]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
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
        [ImportingConstructor]
        public DocumentEvent(
            XDocument document,
            [ImportMany] IEnumerable<IEventInstanceProvider> providers)
            : base (document)
        {
            Contract.Requires<ArgumentNullException>(document != null);
            Contract.Requires<ArgumentNullException>(providers != null);

            this.providers = providers;
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
