using System;
using System.Diagnostics.Contracts;

using NXKit.DOM2.Events;

namespace NXKit
{

    public class EventListener :
        IEventListener
    {

        /// <summary>
        /// Registers a handler action for the given event.
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="target"></param>
        /// <param name="action"></param>
        /// <param name="useCapture"></param>
        public static void Register(string eventType, IEventTarget target, Action<IEvent> action, bool useCapture)
        {
            Contract.Requires<ArgumentNullException>(eventType != null);
            Contract.Requires<ArgumentNullException>(target != null);
            Contract.Requires<ArgumentNullException>(action != null);

            target.AddEventListener(eventType, new EventListener(action), useCapture);
        }

        Action<IEvent> action;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="action"></param>
        EventListener(Action<IEvent> action)
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
