using System;
using System.Diagnostics.Contracts;

namespace NXKit.DOMEvents
{

    public class EventListener :
        IEventListener
    {

        Action<Event> action;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="action"></param>
        public EventListener(Action<Event> action)
        {
            Contract.Requires<ArgumentNullException>(action != null);

            this.action = action;
        }

        void IEventListener.HandleEvent(Event @event)
        {
            action(@event);
        }

    }

}
