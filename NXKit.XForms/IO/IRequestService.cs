using System;
using System.Diagnostics.Contracts;

namespace NXKit.XForms.IO
{

    /// <summary>
    /// Provides a service for dispatching <see cref="Request"/>s and receiving <see cref="Response"/>s.
    /// </summary>
    [ContractClass(typeof(IRequestService_Contract))]
    public interface IRequestService
    {

        /// <summary>
        /// Submits the given <see cref="Request"/> and obtains a new <see cref="Response"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Response Submit(Request request);

    }

    [ContractClassFor(typeof(IRequestService))]
    abstract class IRequestService_Contract :
        IRequestService
    {

        public Response Submit(Request request)
        {
            Contract.Requires<ArgumentNullException>(request != null);
            Contract.Ensures(Contract.Result<Response>() != null);
            throw new NotImplementedException();
        }

    }

}
