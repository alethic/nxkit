namespace NXKit.Events
{

    public class EventsEventListenerVisual : StructuralVisual, IEventHandlerVisual
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
