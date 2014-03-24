namespace NXKit.DOMEvents
{

    public interface IEventTarget
    {

        void AddEventListener(string type, IEventListener listener, bool useCapture);

        void RemoveEventListener(string type, IEventListener listener, bool useCapture);

        bool DispatchEvent(IEvent evt);

        void AddEventHandler<T>(string type, bool useCapture, EventHandlerDelegate handler)
            where T : IEvent;

    }

}
