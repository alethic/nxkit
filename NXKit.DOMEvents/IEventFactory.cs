namespace NXKit.DOMEvents
{

    /// <summary>
    /// Provides event instances configured as appropriate.
    /// </summary>
    public interface IEventFactory
    {

        /// <summary>
        /// Creates a new event for the given event type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Event CreateEvent(string type);

        /// <summary>
        /// Creates a new event for the given event type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        T CreateEvent<T>(string type)
            where T : Event;

    }

}
