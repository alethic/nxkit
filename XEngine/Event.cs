using System;

namespace XEngine.Forms
{

    public class Event
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public Event()
        {
            EventPhase = EventPhase.Uninitialized;
            TimeStamp = (ulong)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="canBubble"></param>
        /// <param name="cancelable"></param>
        public Event(string type, bool canBubble, bool cancelable)
            : this()
        {
            InitEvent(type, canBubble, cancelable);
        }

        public string Type { get; private set; }

        public IEventTarget Target { get; internal set; }

        public IEventTarget CurrentTarget { get; internal set; }

        public EventPhase EventPhase { get; internal set; }

        public bool Bubbles { get; private set; }

        public bool Cancelable { get; private set; }

        public ulong TimeStamp { get; private set; }

        internal bool StopPropagationSet { get; private set; }

        internal bool PreventDefaultSet { get; private set; }

        public void StopPropagation()
        {
            StopPropagationSet = true;
        }

        public void PreventDefault()
        {
            PreventDefaultSet = true;
        }

        public void InitEvent(string type, bool canBubble, bool cancelable)
        {
            if (EventPhase != EventPhase.Uninitialized)
                throw new InvalidOperationException();

            Type = type;
            Bubbles = canBubble;
            Cancelable = Cancelable;
        }

    }

}
