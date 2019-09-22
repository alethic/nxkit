using System;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// Signifies a DOM event to be raised.
    /// </summary>
    public class DOMEventException :
        Exception
    {

        readonly string type;
        readonly object contextInfo;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="contextInfo"></param>
        public DOMEventException(string type, object contextInfo)
            : this(type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            this.contextInfo = contextInfo;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        public DOMEventException(string type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            this.type = type;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public DOMEventException(string type, string message)
            : base(message)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            this.type = type;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="innerException"></param>
        public DOMEventException(string type, Exception innerException)
            : base(innerException.Message, innerException)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (innerException == null)
                throw new ArgumentNullException(nameof(innerException));

            this.type = type;
        }

        /// <summary>
        /// Gets the type of event to be raised.
        /// </summary>
        public string EventType
        {
            get { return type; }
        }

        public object ContextInfo
        {
            get { return contextInfo; }
        }

    }

}
