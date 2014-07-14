using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.DOMEvents;
using NXKit.Xml;


namespace NXKit.XMLEvents
{

    [Extension("{http://www.w3.org/2001/xml-events}action")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
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

        public void HandleEvent(Event evt)
        {
            var handlers = Element
                .Elements()
                .SelectMany(i => i.Interfaces<IEventHandler>());

            foreach (var handler in handlers)
                handler.HandleEvent(evt);
        }

    }

}
