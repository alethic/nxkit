namespace NXKit.DOMEvents
{

    /// <summary>
    /// The DocumentEvent interface provides a mechanism by which the user can create an Event object of a type supported by the implementation. The DocumentEvent interface must be implemented on the same object that implements the Document interface.
    /// </summary>
    public interface INXDocumentEvent : IDocumentExtension
    {

        /// <summary>
        /// Creates an event object of the type specified. Returns the newly created object.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Event CreateEvent(string type);

        /// <summary>
        /// Creates an event object of the type specified. Returns the newly created object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        T CreateEvent<T>(string type)
            where T : Event;

    }

}
