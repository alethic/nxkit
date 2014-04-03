using System;
using System.Diagnostics.Contracts;
using NXKit.DOMEvents;

namespace NXKit.XmlEvents
{

    /// <summary>
    /// Listens for a given event on an element.
    /// </summary>
    public class ElementEventListener :
        IEventListener
    {

        readonly NXElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public ElementEventListener(NXElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.element = element;
        }

    }

}
