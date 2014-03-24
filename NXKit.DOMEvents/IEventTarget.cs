namespace NXKit.DOMEvents
{

    public interface IEventTarget
    {

        void AddEventListener(string type, IEventListener listener, bool useCapture);

        void RemoveEventListener(string type, IEventListener listener, bool useCapture);

        void DispatchEvent(Event evt);

        void AddEventHandler<T>(string type, bool useCapture, EventHandlerDelegate handler)
            where T : Event;

    }

}
