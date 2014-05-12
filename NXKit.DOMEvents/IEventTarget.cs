namespace NXKit.DOMEvents
{

    public interface IEventTarget
    {

        void AddEventListener(string type, IEventListener listener, bool useCapture);

        void AddEventListener(string type, IEventListener listener);

        void RemoveEventListener(string type, IEventListener listener, bool useCapture);

        void RemoveEventListener(string type, IEventListener listener);

        bool DispatchEvent(Event evt);

    }

}
