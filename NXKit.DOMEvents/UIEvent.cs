using System;
using System.Diagnostics.Contracts;

namespace NXKit.DOMEvents
{

    public class UIEvent :
        Event
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public UIEvent(NXDocumentHost host)
            : base(host)
        {
            Contract.Requires<ArgumentNullException>(host != null);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="type"></param>
        public UIEvent(NXDocumentHost host, string type)
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
        /// <param name="canBubble"></param>
        /// <param name="cancelable"></param>
        /// <param name="view"></param>
        /// <param name="detail"></param>
        public UIEvent(NXDocumentHost host, string type, bool canBubble, bool cancelable, object view, long detail)
            : base(host, type, canBubble, cancelable)
        {
            Contract.Requires<ArgumentNullException>(host != null);
            Contract.Requires<ArgumentNullException>(type != null);
            Contract.Requires<ArgumentNullException>(type.Length > 0);

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
