namespace NXKit.DOMEvents
{

    /// <summary>
    /// NX-specfic event target interface.
    /// </summary>
    [Remote]
    public interface INXEventTarget
    {

        /// <summary>
        /// Dispatches an event of the given type, searching for the default event settings in the container.
        /// </summary>
        /// <param name="type"></param>
        [Remote]
        Event DispatchEvent(string type);

        /// <summary>
        /// Dispatches an event of the given type, searching for the default event settings in the container. Provides
        /// an object for attaching contextual information to the event.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="context"></param>
        Event DispatchEvent(string type, object context);

        /// <summary>
        /// Adds an event handler delegate to be invoked when an event is called.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="useCapture"></param>
        /// <param name="handler"></param>
        void AddEventHandler(string type, bool useCapture, EventHandlerDelegate handler);

        /// <summary>
        /// Adds an event handler delegate to be invoked when an event is called, without using capture.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="handler"></param>
        void AddEventHandler(string type, EventHandlerDelegate handler);

    }

}
