using NXKit.DOMEvents;

namespace NXKit.XMLEvents
{

    /// <summary>
    /// Describes a node which will receive events.
    /// </summary>
    public interface IEventHandler
    {

        /// <summary>
        /// Handles the received event.
        /// </summary>
        /// <param name="evt"></param>
        void HandleEvent(Event evt);

    }

}
