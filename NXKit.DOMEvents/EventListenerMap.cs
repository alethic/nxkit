using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// List of event listeners registered with a particular <see cref="NXNode"/>.
    /// </summary>
    [Serializable]
    internal class EventListenerMap :
        Dictionary<string, List<EventListenerData>>,
        ISerializable
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public EventListenerMap()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public EventListenerMap(SerializationInfo info, StreamingContext context)
            : this()
        {

        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {

        }

    }

}
