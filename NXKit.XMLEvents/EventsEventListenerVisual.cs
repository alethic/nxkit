using NXKit.DOMEvents;

namespace NXKit.XmlEvents
{

    public class EventsEventListenerVisual :
        ContentVisual,
        IEventHandlerVisual
    {

        public override string Id
        {
            get { return Document.GetElementId(Element); }
        }

        public void Handle(IEvent ev)
        {

        }

    }

}
