using System;
using System.Diagnostics.Contracts;
using System.Dynamic;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// The Event interface provides basic contextual information about an event to all registered event handlers.
    /// Specific events can also implement other derived interfaces, for example the UIEvent and MouseEvent interfaces.
    /// </summary>
    public class Event
    {

        NXDocumentHost host;
        string type;
        IEventTarget target;
        IEventTarget currentTarget;
        EventPhase eventPhase;
        bool bubbles;
        bool cancelable;
        ulong timeStamp;
        bool defaultPrevented;
        bool isTrusted;
        bool stopPropagationSet;
        bool stopImmediatePropagationSet;
        bool preventDefaultSet;
        dynamic context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public Event(NXDocumentHost host)
        {
            Contract.Requires<ArgumentNullException>(host != null);

            this.host = host;
            this.eventPhase = EventPhase.Uninitialized;
            this.timeStamp = (ulong)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
            this.context = new ExpandoObject();
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="type"></param>
        public Event(NXDocumentHost host, string type)
            : this(host)
        {
            Contract.Requires<ArgumentNullException>(host != null);
            Contract.Requires<ArgumentNullException>(type != null);

            this.type = type;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="type"></param>
        /// <param name="canBubble"></param>
        /// <param name="cancelable"></param>
        public Event(NXDocumentHost host, string type, bool canBubble, bool cancelable)
            : this(host)
        {
            Contract.Requires<ArgumentNullException>(host != null);
            Contract.Requires<ArgumentNullException>(type != null);
            Contract.Requires<ArgumentNullException>(type.Length > 0);

            InitEvent(type, canBubble, cancelable);
        }

        public void InitEvent(string type, bool canBubble, bool cancelable)
        {
            if (EventPhase != EventPhase.Uninitialized)
                throw new InvalidOperationException();

            this.type = type;
            this.bubbles = canBubble;
            this.cancelable = cancelable;
            this.defaultPrevented = false;
            this.isTrusted = false;
            this.stopPropagationSet = false;
            this.stopImmediatePropagationSet = false;
            this.preventDefaultSet = false;
        }

        /// <summary>
        /// Gets the <see cref="NXDocumentHost"/> that owns the event.
        /// </summary>
        public NXDocumentHost Host
        {
            get { return host; }
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

        public bool StopPropagationSet
        {
            get { return stopPropagationSet; }
        }

        public bool PreventDefaultSet
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

        public void StopImmediatePropagation()
        {
            stopImmediatePropagationSet = true;
        }

        public bool DefaultPrevented
        {
            get { return defaultPrevented; }
        }

        public bool IsTrusted
        {
            get { return isTrusted; }
        }

        /// <summary>
        /// Gets an object that can be used to attach arbitrary dynamic information to an <see cref="Event"/>.
        /// </summary>
        public dynamic Context
        {
            get { return context; }
            set { context = value; }
        }

    }

}
