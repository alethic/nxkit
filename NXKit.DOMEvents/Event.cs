using System;
using System.Diagnostics.Contracts;
using System.Dynamic;
using System.Xml.Linq;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// The Event interface provides basic contextual information about an event to all registered event handlers.
    /// Specific events can also implement other derived interfaces, for example the UIEvent and MouseEvent interfaces.
    /// </summary>
    public class Event
    {

        readonly Document host;
        internal string type;
        internal bool bubbles;
        internal bool cancelable;
        internal XNode target;
        internal XNode currentTarget;
        internal EventPhase eventPhase;
        internal bool stopPropagation;
        internal bool stopImmediatePropagation;
        internal bool canceled;
        internal bool initialized;
        internal bool dispatch;
        internal bool isTrusted;
        internal ulong timeStamp;
        dynamic context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public Event(Document host)
        {
            Contract.Requires<ArgumentNullException>(host != null);

            this.initialized = false;
            this.host = host;
            this.eventPhase = EventPhase.None;
            this.timeStamp = (ulong)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
            this.isTrusted = false;
            this.context = new ExpandoObject();
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="type"></param>
        public Event(Document host, string type)
            : this(host)
        {
            if (host == null)
                throw new ArgumentNullException(nameof(host));

            this.type = type ?? throw new ArgumentNullException(nameof(type));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="type"></param>
        /// <param name="canBubble"></param>
        /// <param name="cancelable"></param>
        public Event(Document host, string type, bool canBubble, bool cancelable)
            : this(host)
        {
            if (host == null)
                throw new ArgumentNullException(nameof(host));
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (type.Length <= 0)
                throw new ArgumentOutOfRangeException(nameof(type));

            InitEvent(type, canBubble, cancelable);
        }

        public void InitEvent(string type, bool bubbles, bool cancelable)
        {
            this.initialized = true;
            if (this.dispatch)
                return;

            this.stopPropagation = false;
            this.stopImmediatePropagation = false;
            this.canceled = false;
            this.isTrusted = false;
            this.target = null;
            this.type = type;
            this.bubbles = bubbles;
            this.cancelable = cancelable;
        }

        /// <summary>
        /// Gets the <see cref="Document"/> that owns the event.
        /// </summary>
        public Document Host
        {
            get { return host; }
        }

        public string Type
        {
            get { return type; }
        }

        public XNode Target
        {
            get { return target; }
        }

        public XNode CurrentTarget
        {
            get { return currentTarget; }
        }

        public EventPhase EventPhase
        {
            get { return eventPhase; }
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

        public void StopPropagation()
        {
            stopPropagation = true;
        }

        public void StopImmediatePropagation()
        {
            stopImmediatePropagation = true;
        }

        public bool PropagationStopped
        {
            get { return stopPropagation; }
        }

        public bool ImmediatePropagationStopped
        {
            get { return stopImmediatePropagation; }
        }

        public void PreventDefault()
        {
            canceled = true;
        }

        public bool DefaultPrevented
        {
            get { return canceled; }
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
