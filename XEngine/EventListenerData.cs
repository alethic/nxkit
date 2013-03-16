namespace XEngine.Forms
{

    /// <summary>
    /// Information required to associate an <see cref="IEventListener"/> with an event on a particular <see cref="Visual"/>.
    /// </summary>
    internal class EventListenerData
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="useCapture"></param>
        public EventListenerData(IEventListener listener, bool useCapture)
        {
            Listener = listener;
            UseCapture = UseCapture;
        }

        /// <summary>
        /// Gets the <see cref="IEventListener"/> registered.
        /// </summary>
        public IEventListener Listener { get; private set; }

        /// <summary>
        /// Gets whether the <see cref="IEventListener"/> should process the capture phase.
        /// </summary>
        public bool UseCapture { get; private set; }

    }

}
