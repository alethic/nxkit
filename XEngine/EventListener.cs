using System;
using System.Reflection;

namespace XEngine.Forms
{

    public class EventListener<T> : IEventListener
        where T : Event
    {

        /// <summary>
        /// Registers a handler action for the given <see cref="Event"/>.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="action"></param>
        /// <param name="useCapture"></param>
        public static void Register(IEventTarget target, Action<T> action, bool useCapture)
        {
            string name = null;

            var nameField = typeof(T).GetField("Name", BindingFlags.Static | BindingFlags.Public);
            if (nameField != null)
                name = (string)nameField.GetValue(null);

            if (name == null)
                throw new Exception();

            target.AddEventListener(name, new EventListener<T>(action), useCapture);
        }

        private Action<T> action;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="action"></param>
        private EventListener(Action<T> action)
        {
            this.action = action;
        }

        void IEventListener.HandleEvent(Event @event)
        {
            action((T)@event);
        }

    }

}
