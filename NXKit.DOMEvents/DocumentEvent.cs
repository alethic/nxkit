using NXKit.DOM;

namespace NXKit.DOMEvents
{

    public class DocumentEvent :
        IDocumentEvent
    {

        public Event CreateEvent(string eventInterface)
        {
            switch (eventInterface)
            {
                case "Event":
                    return new Event();
                case "UIEvent":
                    return new UIEvent();
                default:
                    throw new DOMException();
            }
        }

    }

}
