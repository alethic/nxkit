using NXKit.DOMEvents;

namespace NXKit.XmlEvents
{

    /// <summary>
    /// Marks a <see cref="Visual"/> as capable of handling DOM events.
    /// </summary>
    public interface IEventHandlerVisual
    {

        void Handle(IEvent ev);

    }

}
