using NXKit.DOMEvents;

namespace NXKit.XMLEvents
{

    /// <summary>
    /// Describes a node which will receive events.
    /// </summary>
    public interface IEventHandler
    {

        void Handle(Event evt);

    }

}
