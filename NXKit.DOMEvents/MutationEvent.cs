using System;
using System.Diagnostics.Contracts;

namespace NXKit.DOMEvents
{

    public class MutationEvent :
        Event
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public MutationEvent()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        public MutationEvent(string type)
            : base(type)
        {
            Contract.Requires<ArgumentNullException>(type != null);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="bubbles"></param>
        /// <param name="cancelable"></param>
        public MutationEvent(string type, bool bubbles, bool cancelable)
            : base(type, bubbles, cancelable)
        {
            Contract.Requires<ArgumentNullException>(type != null);
            Contract.Requires<ArgumentNullException>(type.Length > 0);

            InitMutationEvent(type, bubbles, cancelable);
        }

        public void InitMutationEvent(string type, bool bubbles, bool cancelable)
        {
            base.InitEvent(type, bubbles, cancelable);
        }

    }

}
