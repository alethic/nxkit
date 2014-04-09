using System;
using System.Diagnostics.Contracts;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// Signifies a DOM event to be raised.
    /// </summary>
    public class DOMEventException :
        Exception
    {

        readonly string type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        public DOMEventException(string type)
        {
            Contract.Requires<ArgumentNullException>(type != null);

            this.type = type;
        }

        /// <summary>
        /// Gets the type of event to be raised.
        /// </summary>
        public string EventType
        {
            get { return type; }
        }

    }

}
