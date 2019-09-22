using System;

namespace NXKit.DOMEvents
{

    public class FocusEvent :
        Event
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="host"></param>
        public FocusEvent(Document host)
            : base(host)
        {
            if (host == null)
                throw new ArgumentNullException(nameof(host));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="type"></param>
        public FocusEvent(Document host, string type)
            : base(host, type)
        {
            if (host == null)
                throw new ArgumentNullException(nameof(host));
            if (type == null)
                throw new ArgumentNullException(nameof(type));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="type"></param>
        /// <param name="canBubble"></param>
        /// <param name="cancelable"></param>
        public FocusEvent(Document host, string type, bool canBubble, bool cancelable)
            : base(host, type, canBubble, cancelable)
        {
            if (host == null)
                throw new ArgumentNullException(nameof(host));
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (type.Length < 1)
                throw new ArgumentOutOfRangeException(nameof(type));

            InitFocusEvent(type, canBubble, cancelable);
        }

        public void InitFocusEvent(string type, bool canBubble, bool cancelable)
        {
            base.InitEvent(type, canBubble, cancelable);
        }

    }

}
