using System;
using System.Diagnostics.Contracts;

namespace NXKit.XForms.IO
{

    /// <summary>
    /// Provides a service for dispatching <see cref="ModelRequest"/>s and receiving <see cref="ModelResponse"/>s.
    /// </summary>
    [ContractClass(typeof(IRequestService_Contract))]
    public interface IModelRequestService
    {

        /// <summary>
        /// Submits the given <see cref="ModelRequest"/> and obtains a new <see cref="ModelResponse"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ModelResponse Submit(ModelRequest request);

    }

    [ContractClassFor(typeof(IModelRequestService))]
    abstract class IRequestService_Contract :
        IModelRequestService
    {

        public ModelResponse Submit(ModelRequest request)
        {
            Contract.Requires<ArgumentNullException>(request != null);
            Contract.Ensures(Contract.Result<ModelResponse>() != null);
            throw new NotImplementedException();
        }

    }

}
