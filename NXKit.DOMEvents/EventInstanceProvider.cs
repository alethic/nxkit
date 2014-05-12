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
            switch (eventInterface.ToLowerInvariant())
            {
                case "event":
                case "events":
                    return new Event(host);
                case "uievent":
                case "uievents":
                    return new UIEvent(host);
                case "focusevent":
                    return new FocusEvent(host);
                case "mutationevent":
                    return new MutationEvent(host);
                default:
                    return null;
            }
        }

    }

}
