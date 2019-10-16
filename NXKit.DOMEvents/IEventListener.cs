namespace NXKit.DOMEvents
{

    public interface IEventListener : IElementExtension
    {

        void HandleEvent(Event evt);

    }

}
