using NXKit.DOM2.Events;

namespace NXKit.Events
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
