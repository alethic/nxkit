using System;
using System.ComponentModel.Composition;

using NXKit.Composition;

namespace NXKit.DOMEvents
{

    [Export(typeof(IEventInstanceProvider))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Host)]
    public class EventInstanceProvider :
        IEventInstanceProvider
    {

        readonly Func<Document> host;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="host"></param>
        [ImportingConstructor]
        public EventInstanceProvider(Func<Document> host)
        {
            this.host = host ?? throw new ArgumentNullException(nameof(host));
        }

        public Event CreateEvent(string eventInterface)
        {
            var host = this.host();
            if (host == null)
                throw new NullReferenceException();

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
