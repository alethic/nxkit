namespace NXKit.DOMEvents
{

    /// <summary>
    /// Provides <see cref="Event"/> instances.
    /// </summary>
    public interface IEventInstanceProvider
    {

        Event CreateEvent(string eventInterface);

    }

}
