using System;

namespace NXKit.DOMEvents
{

    public class UIEvent :
        Event
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public UIEvent(Document host)
            : base(host)
        {
            if (host == null)
                throw new ArgumentNullException(nameof(host));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="type"></param>
        public UIEvent(Document host, string type)
            : base(host, type)
        {
            if (host == null)
                throw new ArgumentNullException(nameof(host));
            if (type == null)
                throw new ArgumentNullException(nameof(type));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="type"></param>
        /// <param name="canBubble"></param>
        /// <param name="cancelable"></param>
        /// <param name="view"></param>
        /// <param name="detail"></param>
        public UIEvent(Document host, string type, bool canBubble, bool cancelable, object view, long detail)
            : base(host, type, canBubble, cancelable)
        {
            if (host == null)
                throw new ArgumentNullException(nameof(host));
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (type.Length < 1)
                throw new ArgumentOutOfRangeException(nameof(type));

            InitUIEvent(type, canBubble, cancelable, view, detail);
        }

        public void InitUIEvent(string type, bool canBubble, bool cancelable, object view, long detail)
        {
            throw new NotImplementedException();
        }

        public object View
        {
            get { throw new NotImplementedException(); }
        }

        public long Detail
        {
            get { throw new NotImplementedException(); }
        }

    }

}
