using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using NXKit.Composition;

namespace NXKit.DOMEvents
{

    [ScopeExport(typeof(IEventInstanceProvider), Scope.Host)]
    public class EventInstanceProvider :
        IEventInstanceProvider
    {

        readonly NXDocumentHost host;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="host"></param>
        [ImportingConstructor]
        public EventInstanceProvider(NXDocumentHost host)
        {
            Contract.Requires<ArgumentNullException>(host != null);

            this.host = host;
        }

        public Event CreateEvent(string eventInterface)
        {
            switch (eventInterface)
            {
                case "Event":
                    return new Event(host);
                case "UIEvent":
                    return new UIEvent(host);
                case "FocusEvent":
                    return new FocusEvent(host);
                case "MutationEvent":
                    return new MutationEvent(host);
                default:
                    return null;
            }
        }

    }

}
