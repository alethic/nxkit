using System;
using System.Diagnostics.Contracts;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// Describes an event.
    /// </summary>
    public class UIEventInfo :
        EventInfo
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="bubbles"></param>
        /// <param name="cancelable"></param>
        public UIEventInfo(string type, bool bubbles, bool cancelable)
            : base(type, "UIEvent", bubbles, cancelable)
        {
            Contract.Requires<ArgumentNullException>(type != null);
            Contract.Requires<ArgumentNullException>(type.Length > 0);
        }

    }

}
