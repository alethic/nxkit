using System;
using System.Diagnostics.Contracts;

namespace NXKit.XForms.IO
{

    /// <summary>
    /// Describes a class capable of handling a <see cref="ModelRequest"/> and generating a <see cref="ModelResponse"/>.
    /// </summary>
    [ContractClass(typeof(IRequestHandler_Contract))]
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

    [ContractClassFor(typeof(IModelRequestHandler))]
    abstract class IRequestHandler_Contract:
        IModelRequestHandler
    {

        public Priority CanSubmit(ModelRequest request)
        {
            Contract.Requires<ArgumentNullException>(request != null);
            throw new NotImplementedException();
        }

        public ModelResponse Submit(ModelRequest request)
        {
            Contract.Requires<ArgumentNullException>(request != null);
            Contract.Ensures(Contract.Result<ModelResponse>() != null);
            throw new NotImplementedException();
        }

    }

}
