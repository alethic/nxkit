using System;
using System.Xml.Linq;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// Signifies a DOM event to be raised by the specified DOM element.
    /// </summary>
    public class DOMTargetEventException :
        DOMEventException
    {

        readonly XElement target;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="type"></param>
        /// <param name="contextInfo"></param>
        public DOMTargetEventException(XElement target, string type, object contextInfo)
            : base(type, contextInfo)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            this.target = target ?? throw new ArgumentNullException(nameof(target));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="type"></param>
        public DOMTargetEventException(XElement target, string type)
            : base(type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            this.target = target ?? throw new ArgumentNullException(nameof(target));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public DOMTargetEventException(XElement target, string type, string message)
            : base(type, message)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            this.target = target ?? throw new ArgumentNullException(nameof(target));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="type"></param>
        /// <param name="innerException"></param>
        public DOMTargetEventException(XElement target, string type, Exception innerException)
            : base(type, innerException)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (innerException == null)
                throw new ArgumentNullException(nameof(innerException));

            this.target = target ?? throw new ArgumentNullException(nameof(target));
        }

        /// <summary>
        /// Gets the target of the event to be raised.
        /// </summary>
        public XElement Target
        {
            get { return target; }
        }

    }

}
