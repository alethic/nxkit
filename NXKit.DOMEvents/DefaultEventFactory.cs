using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;

using NXKit.Composition;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// Implements the <see cref="IEventFactory"/> interface.
    /// </summary>
    [Export(typeof(IEventFactory))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Host)]
    public class DefaultEventFactory :
        IEventFactory
    {

        readonly IEnumerable<IEventProvider> providers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="providers"></param>
        [ImportingConstructor]
        public DefaultEventFactory(
            [ImportMany] IEnumerable<IEventProvider> providers)
        {
            Contract.Requires<ArgumentNullException>(providers != null);

            this.providers = providers;
        }

        public Event CreateEvent(string type)
        {
            return providers
                .Select(i => i.CreateEvent(type))
                .FirstOrDefault(i => i != null);
        }

        public T CreateEvent<T>(string type)
            where T : Event
        {
            return (T)CreateEvent(type);
        }

    }

}
