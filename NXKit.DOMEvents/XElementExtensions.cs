using System;
using System.Diagnostics.Contracts;
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
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(type));

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
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(type));

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
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(evt != null);

            var target = element.InterfaceOrDefault<EventTarget>();
            if (target == null)
                throw new NullReferenceException();

            target.Dispatch(evt);
        }

    }

}
