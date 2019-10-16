using System;

using NXKit.Composition;

namespace NXKit.DOMEvents
{

    [Export(typeof(IEventInstanceProvider), CompositionScope.Host)]
    public class EventInstanceProvider :
        IEventInstanceProvider
    {

        readonly DocumentEnvironment environment;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="environment"></param>
        public EventInstanceProvider(DocumentEnvironment environment)
        {
            this.environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        public Event CreateEvent(string eventInterface)
        {
            var host = environment.GetHost();
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
