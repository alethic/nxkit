namespace NXKit.IO
{

    /// <summary>
    /// Provides a service for dispatching <see cref="IORequest"/>s and receiving <see cref="IOResponse"/>s.
    /// </summary>
    public interface IIOService
    {

        /// <summary>
        /// Submits the given <see cref="IORequest"/> and obtains a new <see cref="IOResponse"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        IOResponse Send(IORequest request);

    }

}
