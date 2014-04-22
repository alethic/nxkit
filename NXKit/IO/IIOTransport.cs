using System;
using System.Diagnostics.Contracts;

namespace NXKit.IO
{

    /// <summary>
    /// Describes a class capable of handling a <see cref="IORequest"/> and generating a <see cref="IOResponse"/>.
    /// </summary>
    [ContractClass(typeof(IRequestHandler_Contract))]
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

    [ContractClassFor(typeof(IIOTransport))]
    abstract class IRequestHandler_Contract:
        IIOTransport
    {

        public Priority CanSend(IORequest request)
        {
            Contract.Requires<ArgumentNullException>(request != null);
            throw new NotImplementedException();
        }

        public IOResponse Submit(IORequest request)
        {
            Contract.Requires<ArgumentNullException>(request != null);
            Contract.Ensures(Contract.Result<IOResponse>() != null);
            throw new NotImplementedException();
        }

    }

}
