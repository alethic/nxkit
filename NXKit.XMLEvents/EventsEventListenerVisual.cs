using System.Xml.Linq;

using NXKit.DOMEvents;

namespace NXKit.XmlEvents
{

    public class EventsEventListenerVisual :
        NXElement
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public EventsEventListenerVisual(XElement element)
            : base(element)
        {

        }

        public override string Id
        {
            get { return Document.GetElementId(Xml); }
        }

        public void Handle(Event ev)
        {

        }

    }

}
