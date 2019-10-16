using System;
using System.Collections.Generic;
using System.Linq;

using NXKit.Composition;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// Implements the <see cref="IEventFactory"/> interface.
    /// </summary>
    [Export(typeof(IEventFactory), CompositionScope.Host)]
    public class DefaultEventFactory :
        IEventFactory
    {

        readonly IEnumerable<IEventProvider> providers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="providers"></param>
        public DefaultEventFactory(IEnumerable<IEventProvider> providers)
        {
            this.providers = providers ?? throw new ArgumentNullException(nameof(providers));
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
