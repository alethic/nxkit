namespace NXKit.DOM2.Events
{

    public interface IUIEvent :
        IEvent
    {

        object View { get; }

        long Detail { get; }

        void InitUIEvent(string type, bool canBubble, bool cancelable, object view, long detail);

    }

}
