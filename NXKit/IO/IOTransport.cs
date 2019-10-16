namespace NXKit.IO
{

    /// <summary>
    /// Base implementation of the <see cref="IIOTransport"/> interface.
    /// </summary>
    public abstract class IOTransport :
        IIOTransport
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public IOTransport()
        {

        }

        /// <summary>
        /// Return <c>true</c> if your <see cref="IIOTransport"/> supports the given <see cref="IORequest"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public abstract Priority CanSend(IORequest request);

        /// <summary>
        /// Handles a submitted request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public abstract IOResponse Submit(IORequest request);

    }

}
