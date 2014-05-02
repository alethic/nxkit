using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

using NXKit.DOMEvents;
using NXKit.Xml;
using NXKit.XMLEvents;

namespace NXKit.XForms
{

    [Interface("{http://www.w3.org/2002/xforms}action")]
    public class Action :
        ElementExtension,
        IEventHandler
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Action(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

        public void HandleEvent(Event ev)
        {
            var handlers = Element
                .Elements()
                .SelectMany(i => i.Interfaces<IEventHandler>());

            foreach (var handler in handlers)
                handler.HandleEvent(ev);
        }

    }

}
