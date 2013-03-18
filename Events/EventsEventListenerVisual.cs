using System.Xml.Linq;

namespace NXKit.Events
{

    public class EventsEventListenerVisual : StructuralVisual, IEventHandlerVisual
    {

        public override string Id
        {
            get { return Engine.GetElementId(Element); }
        }

        public void Handle(Event ev)
        {

        }

    }

}
