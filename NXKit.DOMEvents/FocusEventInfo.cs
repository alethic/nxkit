using System;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// Describes an event.
    /// </summary>
    public class FocusEventInfo :
        EventInfo
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="bubbles"></param>
        /// <param name="cancelable"></param>
        public FocusEventInfo(string type, bool bubbles, bool cancelable)
            : base(type, "FocusEvent", bubbles, cancelable)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (type.Length < 1)
                throw new ArgumentOutOfRangeException(nameof(type));
        }

    }

}
