using System;
using System.Diagnostics.Contracts;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// Describes an event.
    /// </summary>
    public class MutationEventInfo :
        EventInfo
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="bubbles"></param>
        /// <param name="cancelable"></param>
        public MutationEventInfo(string type, bool bubbles, bool cancelable)
            : base(type, "MutationEvent", bubbles, cancelable)
        {
            Contract.Requires<ArgumentNullException>(type != null);
            Contract.Requires<ArgumentNullException>(type.Length > 0);
        }

    }

}
