namespace NXKit.IO
{

    /// <summary>
    /// Describes a class capable of handling a <see cref="IORequest"/> and generating a <see cref="IOResponse"/>.
    /// </summary>
    public interface IIOTransport
    {

        /// <summary>
        /// Returns <c>true</c> if the handler can submit the specified request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Priority CanSend(IORequest request);

        /// <summary>
        /// Initiates the submission.
        /// </summary>
        /// <param name="elementrequest"></param>
        /// <returns></returns>
        IOResponse Submit(IORequest request);

    }

}
