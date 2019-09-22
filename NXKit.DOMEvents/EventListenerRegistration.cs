using System;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// Describes a <see cref="IEventListener"/>s registration on a <see cref="EventTarget"/>.
    /// </summary>
    public class EventListenerRegistration
    {

        readonly string eventType;
        readonly IEventListener listener;
        readonly bool capture;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="listener"></param>
        /// <param name="capture"></param>
        public EventListenerRegistration(string eventType,IEventListener listener, bool capture )
        {
            if (string.IsNullOrWhiteSpace(eventType))
                throw new ArgumentNullException(nameof(eventType));

            this.eventType = eventType;
            this.listener = listener ?? throw new ArgumentNullException(nameof(listener));
            this.capture = capture;
        }

        /// <summary>
        /// Gets the event type.
        /// </summary>
        public string EventType
        {
            get { return eventType; }
        }

        /// <summary>
        /// Gets the registered <see cref="IEventListener"/>.
        /// </summary>
        public IEventListener Listener
        {
            get { return listener; }
        }

        /// <summary>
        /// Gets whether or not the <see cref="IEventListener"/> is registered for the 'capture' phase.
        /// </summary>
        public bool Capture
        {
            get { return capture; }
        }

        public override bool Equals(object obj)
        {
            var other = obj as EventListenerRegistration;
            if (other == null)
                return false;

            return
                object.Equals(eventType, other.eventType) &&
                object.Equals(capture, other.capture) &&
                object.Equals(listener, other.listener);
        }

        public override int GetHashCode()
        {
            return
                eventType.GetHashCode() ^
                capture.GetHashCode() ^
                listener.GetHashCode();
        }

    }

}
