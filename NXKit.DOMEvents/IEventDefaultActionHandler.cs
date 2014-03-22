namespace NXKit.DOMEvents
{

    /// <summary>
    /// <see cref="Visual"/>s implementing this interface provide a default action for the specified event type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEventDefaultActionHandler<T>
        where T : IEvent
    {

        /// <summary>
        /// Invoked to execute the default action.
        /// </summary>
        /// <param name="evt"></param>
        void DefaultAction(T evt);

    }

}
