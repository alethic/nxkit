namespace NXKit.XForms.IO
{

    /// <summary>
    /// Provides a service for dispatching <see cref="ModelRequest"/>s and receiving <see cref="ModelResponse"/>s.
    /// </summary>
    public interface IModelRequestService
    {

        /// <summary>
        /// Submits the given <see cref="ModelRequest"/> and obtains a new <see cref="ModelResponse"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ModelResponse Submit(ModelRequest request);

    }

}
