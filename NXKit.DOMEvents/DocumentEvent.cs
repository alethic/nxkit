using NXKit.DOM;

namespace NXKit.DOMEvents
{

    public class DocumentEvent :
        IDocumentEvent
    {

        public IEvent CreateEvent(string eventInterface)
        {
            switch (eventInterface)
            {
                case "Event":
                    return new Event();
                default:
                    throw new DOMException();
            }
        }

    }

}
