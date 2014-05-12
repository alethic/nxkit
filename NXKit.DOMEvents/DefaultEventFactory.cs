using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using NXKit.Composition;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// Implements the <see cref="IEventFactory"/> interface.
    /// </summary>
    [ScopeExport(typeof(IEventFactory), Scope.Host)]
    public class DefaultEventFactory :
        IEventFactory
    {

        [ContractInvariantMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        private void ObjectInvariant()
        {
            Contract.Invariant(providers != null);
        }


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
