namespace NXKit.Events
{

    /// <summary>
    /// Marks a <see cref="Visual"/> as capable of handling DOM events.
    /// </summary>
    public interface IEventHandlerVisual
    {

        void Handle(Event ev);

    }

}
