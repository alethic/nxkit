using System;

namespace NXKit.DOMEvents
{

    public class MutationEvent :
        Event
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="host"></param>
        public MutationEvent(Document host)
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
        public MutationEvent(Document host, string type)
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
        /// <param name="bubbles"></param>
        /// <param name="cancelable"></param>
        public MutationEvent(Document host, string type, bool bubbles, bool cancelable)
            : base(host, type, bubbles, cancelable)
        {
            if (host == null)
                throw new ArgumentNullException(nameof(host));
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (type.Length < 1)
                throw new ArgumentOutOfRangeException(nameof(type));

            InitMutationEvent(type, bubbles, cancelable);
        }

        public void InitMutationEvent(string type, bool bubbles, bool cancelable)
        {
            base.InitEvent(type, bubbles, cancelable);
        }

    }

}
