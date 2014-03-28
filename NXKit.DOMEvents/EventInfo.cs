using System;
using System.Diagnostics.Contracts;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// Describes an event.
    /// </summary>
    public class EventInfo
    {

        readonly string type;
        readonly string eventInterface;
        readonly bool canBubble;
        readonly bool cancelable;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="eventInterface"></param>
        /// <param name="bubbles"></param>
        /// <param name="cancelable"></param>
        protected EventInfo(string type, string eventInterface, bool bubbles, bool cancelable)
        {
            Contract.Requires<ArgumentNullException>(type != null);
            Contract.Requires<ArgumentNullException>(type.Length > 0);
            Contract.Requires<ArgumentNullException>(eventInterface != null);
            Contract.Requires<ArgumentNullException>(eventInterface.Length > 0);

            this.type = type;
            this.eventInterface = eventInterface;
            this.canBubble = bubbles;
            this.cancelable = cancelable;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="canBubble"></param>
        /// <param name="cancelable"></param>
        public EventInfo(string type, bool canBubble, bool cancelable)
            :this(type, "Event", canBubble, cancelable)
        {
            Contract.Requires<ArgumentNullException>(type != null);
            Contract.Requires<ArgumentNullException>(type.Length > 0);
        }

        public string Type
        {
            get { return type; }
        }

        public string EventInterface
        {
            get { return eventInterface; }
        }

        public bool Bubbles
        {
            get { return canBubble; }
        }

        public bool Cancelable
        {
            get { return cancelable; }
        }

        public virtual void InitEvent(Event evt)
        {
            evt.InitEvent(type, canBubble, cancelable);
        }

    }

}
