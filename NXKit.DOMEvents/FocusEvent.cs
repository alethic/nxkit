using System;
using System.Diagnostics.Contracts;

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
            Contract.Requires<ArgumentNullException>(host != null);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="type"></param>
        public FocusEvent(Document host, string type)
            : base(host, type)
        {
            Contract.Requires<ArgumentNullException>(host != null);
            Contract.Requires<ArgumentNullException>(type != null);
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
            Contract.Requires<ArgumentNullException>(host != null);
            Contract.Requires<ArgumentNullException>(type != null);
            Contract.Requires<ArgumentNullException>(type.Length > 0);

            InitFocusEvent(type, canBubble, cancelable);
        }

        public void InitFocusEvent(string type, bool canBubble, bool cancelable)
        {
            base.InitEvent(type, canBubble, cancelable);
        }

    }

}
