using System;
using System.Diagnostics.Contracts;

namespace NXKit.DOMEvents
{

    public class EventListener :
        IEventListener
    {

        Action<IEvent> action;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="action"></param>
        public EventListener(Action<IEvent> action)
        {
            Contract.Requires<ArgumentNullException>(action != null);

            this.action = action;
        }

        void IEventListener.HandleEvent(IEvent @event)
        {
            action(@event);
        }

    }

}
