using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.DOMEvents;
using NXKit.Xml;
using NXKit.XMLEvents;

namespace NXKit.XForms
{

    [Extension("{http://www.w3.org/2002/xforms}action")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class Action :
        ElementExtension,
        IEventHandler
    {

        readonly ActionProperties actionProperties;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        [ImportingConstructor]
        public Action(
            XElement element,
            ActionProperties actionProperties)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            this.actionProperties = actionProperties ?? throw new ArgumentNullException(nameof(actionProperties));
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
