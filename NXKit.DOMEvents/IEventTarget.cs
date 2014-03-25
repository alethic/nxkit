namespace NXKit.DOMEvents
{

    public interface IEventTarget
    {

        /// <summary>
        /// Gets the underlying <see cref="NXNode"/> that this interface is provided for.
        /// </summary>
        NXNode Node { get; }

        void AddEventListener(string type, IEventListener listener, bool useCapture);

        void RemoveEventListener(string type, IEventListener listener, bool useCapture);

        void DispatchEvent(Event evt);

        void AddEventHandler(string type, bool useCapture, EventHandlerDelegate handler);

    }

}
