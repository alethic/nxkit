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
        public UIEvent()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        public UIEvent(string type)
            : base(type)
        {
            Contract.Requires<ArgumentNullException>(type != null);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="canBubble"></param>
        /// <param name="cancelable"></param>
        /// <param name="view"></param>
        /// <param name="detail"></param>
        public UIEvent(string type, bool canBubble, bool cancelable, object view, long detail)
            : base(type, canBubble, cancelable)
        {
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
