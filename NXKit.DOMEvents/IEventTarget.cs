using System.Collections.Generic;
namespace NXKit.DOMEvents
{

    public interface IEventTarget
    {

        IEnumerable<IEventListener> GetEventListeners(string type, bool useCapture);

        bool HasEventListener(string type, IEventListener listener, bool useCapture);

        void AddEventListener(string type, IEventListener listener, bool useCapture);

        void RemoveEventListener(string type, IEventListener listener, bool useCapture);

        void DispatchEvent(Event evt);

    }

}
