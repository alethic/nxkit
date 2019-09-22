using System;
using System.Xml.Linq;

using NXKit.Xml;

namespace NXKit.DOMEvents
{

    /// <summary>
    /// Provides various extension methods for <see cref="XElement"/> instances.
    /// </summary>
    public static class XElementExtensions
    {

        /// <summary>
        /// Dispatches the event to this <see cref="XElement"/>.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="type"></param>
        public static void DispatchEvent(this XElement element, string type)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            if (string.IsNullOrWhiteSpace(type))
                throw new ArgumentException("Type must be a non-empty string.", nameof(type));

            var target = element.InterfaceOrDefault<EventTarget>();
            if (target == null)
                throw new NullReferenceException();

            target.Dispatch(type);
        }

        /// <summary>
        /// Dispatches the event to this <see cref="XElement"/>.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="type"></param>
        /// <param name="context"></param>
        public static void DispatchEvent(this XElement element, string type, object context)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            if (string.IsNullOrWhiteSpace(type))
                throw new ArgumentException("Type must be a non-empty string.", nameof(type));

            var target = element.InterfaceOrDefault<EventTarget>();
            if (target == null)
                throw new NullReferenceException();

            target.Dispatch(type, context);
        }

        /// <summary>
        /// Dispatches the event to this <see cref="XElement"/>.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="evt"></param>
        public static void DispatchEvent(this XElement element, Event evt)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            if (evt == null)
                throw new ArgumentNullException(nameof(evt));

            var target = element.InterfaceOrDefault<EventTarget>();
            if (target == null)
                throw new NullReferenceException();

            target.Dispatch(evt);
        }

    }

}
