namespace NXKit.Events
{

    public class EventsEventListenerVisual : ContentVisual, IEventHandlerVisual
    {

        public override string Id
        {
            get { return Document.GetElementId(Element); }
        }

        public void Handle(Event ev)
        {

        }

    }

}
