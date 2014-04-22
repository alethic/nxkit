using System;
using System.Diagnostics.Contracts;

namespace NXKit.IO
{

    /// <summary>
    /// Provides a service for dispatching <see cref="IORequest"/>s and receiving <see cref="IOResponse"/>s.
    /// </summary>
    [ContractClass(typeof(IRequestService_Contract))]
    public interface IIOService
    {

        /// <summary>
        /// Submits the given <see cref="IORequest"/> and obtains a new <see cref="IOResponse"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        IOResponse Send(IORequest request);

    }

    [ContractClassFor(typeof(IIOService))]
    abstract class IRequestService_Contract :
        IIOService
    {

        public IOResponse Send(IORequest request)
        {
            Contract.Requires<ArgumentNullException>(request != null);
            Contract.Ensures(Contract.Result<IOResponse>() != null);
            throw new NotImplementedException();
        }

    }

}
