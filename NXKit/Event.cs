using System;
using System.Diagnostics.Contracts;

using NXKit.DOM2.Events;

namespace NXKit
{

    public class Event :
        IEvent
    {

        string type;
        IEventTarget target;
        IEventTarget currentTarget;
        EventPhase eventPhase;
        bool bubbles;
        bool cancelable;
        ulong timeStamp;
        bool stopPropagationSet;
        internal bool preventDefaultSet;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public Event()
        {
            this.eventPhase = EventPhase.Uninitialized;
            this.timeStamp = (ulong)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
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
            Contract.Requires<ArgumentNullException>(type != null);
            Contract.Requires<ArgumentNullException>(type.Length > 0);

            InitEvent(type, canBubble, cancelable);
        }

        public string Type
        {
            get { return type; }
        }

        public IEventTarget Target
        {
            get { return target; }
            internal set { target = value; }
        }

        public IEventTarget CurrentTarget
        {
            get { return currentTarget; }
            internal set { currentTarget = value; }
        }

        public EventPhase EventPhase
        {
            get { return eventPhase; }
            internal set { eventPhase = value; }
        }

        public bool Bubbles
        {
            get { return bubbles; }
        }

        public bool Cancelable
        {
            get { return cancelable; }
        }

        public ulong TimeStamp
        {
            get { return timeStamp; }
        }

        internal bool StopPropagationSet
        {
            get { return stopPropagationSet; }
        }

        internal bool PreventDefaultSet
        {
            get { return preventDefaultSet; }
        }

        public void StopPropagation()
        {
            stopPropagationSet = true;
        }

        public void PreventDefault()
        {
            preventDefaultSet = true;
        }

        public void InitEvent(string type, bool canBubble, bool cancelable)
        {
            if (EventPhase != EventPhase.Uninitialized)
                throw new InvalidOperationException();

            this.type = type;
            this.bubbles = canBubble;
            this.cancelable = Cancelable;
        }

    }

}
