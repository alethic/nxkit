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
        /// <param name="host"></param>
        public MutationEvent(NXDocumentHost host)
            : base(host)
        {
            Contract.Requires<ArgumentNullException>(host != null);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="type"></param>
        public MutationEvent(NXDocumentHost host, string type)
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
        /// <param name="bubbles"></param>
        /// <param name="cancelable"></param>
        public MutationEvent(NXDocumentHost host, string type, bool bubbles, bool cancelable)
            : base(host, type, bubbles, cancelable)
        {
            Contract.Requires<ArgumentNullException>(host != null);
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
