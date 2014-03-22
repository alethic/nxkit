namespace NXKit.DOM2.Events
{

    public interface IEventTarget
    {

        void AddEventListener(string type, IEventListener listener, bool useCapture);

        void RemoveEventListener(string type, IEventListener listener, bool useCapture);

        bool DispatchEvent(IEvent evt);

    }

}
