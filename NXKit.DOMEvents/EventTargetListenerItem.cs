using System;
using System.Diagnostics.Contracts;

namespace NXKit.DOMEvents
{

    public class EventTargetListenerItem
    {

        readonly string eventType;
        readonly bool useCapture;
        readonly IEventListener listener;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="useCapture"></param>
        /// <param name="listener"></param>
        public EventTargetListenerItem(string eventType, bool useCapture, IEventListener listener)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(eventType));
            Contract.Requires<ArgumentNullException>(listener != null);

            this.eventType = eventType;
            this.useCapture = useCapture;
            this.listener = listener;
        }

        public string EventType
        {
            get { return eventType; }
        }

        public bool UseCapture
        {
            get { return useCapture; }
        }

        public IEventListener Listener
        {
            get { return listener; }
        }

        public override bool Equals(object obj)
        {
            var other = obj as EventTargetListenerItem;
            if (other == null)
                return false;

            return
                object.Equals(eventType, other.eventType) &&
                object.Equals(useCapture, other.useCapture) &&
                object.Equals(listener, other.listener);
        }

        public override int GetHashCode()
        {
            return
                eventType.GetHashCode() ^
                useCapture.GetHashCode() ^
                listener.GetHashCode();
        }

    }

}
