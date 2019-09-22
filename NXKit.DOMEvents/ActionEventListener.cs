using System;

namespace NXKit.DOMEvents
{

    public class ActionEventListener :
        IEventListener
    {

        Action<Event> action;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="action"></param>
        public ActionEventListener(Action<Event> action)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
        }

        void IEventListener.HandleEvent(Event @event)
        {
            action(@event);
        }

    }

}
