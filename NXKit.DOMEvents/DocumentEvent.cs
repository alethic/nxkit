using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;

using NXKit.Composition;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// Provides a <see cref="IDocumentEvent"/> implementation.
    /// </summary>
    [Extension(ExtensionObjectType.Document)]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class DocumentEvent :
         IDocumentEvent
    {

        readonly IEnumerable<IEventInstanceProvider> providers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="providers"></param>
        public DocumentEvent(
            IEnumerable<IEventInstanceProvider> providers)
        {
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
