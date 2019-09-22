namespace NXKit.XForms.IO
{

    /// <summary>
    /// Describes a class capable of handling a <see cref="ModelRequest"/> and generating a <see cref="ModelResponse"/>.
    /// </summary>
    public interface IModelRequestHandler
    {

        /// <summary>
        /// Returns <c>true</c> if the handler can submit the specified request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Priority CanSubmit(ModelRequest request);

        /// <summary>
        /// Initiates the submission.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ModelResponse Submit(ModelRequest request);

    }

}
