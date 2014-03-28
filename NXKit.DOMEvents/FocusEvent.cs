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
        public FocusEvent()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        public FocusEvent(string type)
            : base(type)
        {
            Contract.Requires<ArgumentNullException>(type != null);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="canBubble"></param>
        /// <param name="cancelable"></param>
        public FocusEvent(string type, bool canBubble, bool cancelable)
            : base(type, canBubble, cancelable)
        {
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
