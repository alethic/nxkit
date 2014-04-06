using System;
using System.Diagnostics.Contracts;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// Signifies a DOM event to be raised by the initiating DOM element.
    /// </summary>
    public class DOMEventException :
        Exception
    {

        readonly string type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="eventType"></param>
        public DOMEventException(string eventType)
        {
            Contract.Requires<ArgumentNullException>(eventType != null);

            this.type = eventType;
        }

        /// <summary>
        /// Gets the type of event to be raise.
        /// </summary>
        public string EventType
        {
            get { return type; }
        }

    }

}
