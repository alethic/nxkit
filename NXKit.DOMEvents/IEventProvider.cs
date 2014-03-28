namespace NXKit.DOMEvents
{

    /// <summary>
    /// Provides configured event instances.
    /// </summary>
    public interface IEventProvider
    {

        /// <summary>
        /// Creates a new event of the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Event CreateEvent(string type);

    }

}
