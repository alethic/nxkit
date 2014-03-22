namespace NXKit.DOM.Events
{

    public interface IEvent
    {

        string Type { get; }

        IEventTarget Target { get; }

        IEventTarget CurrentTarget { get; }

        EventPhase EventPhase { get; }

        bool Bubbles { get; }

        bool Cancelable { get; }

        ulong TimeStamp { get; }

        void StopPropagation();

        void PreventDefault();

        void InitEvent(string type, bool canBubble, bool cancelable);

    }

}
