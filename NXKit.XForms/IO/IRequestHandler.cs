using System;
using System.Diagnostics.Contracts;

namespace NXKit.XForms.IO
{

    /// <summary>
    /// Describes a class capable of handling a <see cref="Request"/> and generating a <see cref="Response"/>.
    /// </summary>
    [ContractClass(typeof(IRequestHandler_Contract))]
    public interface IRequestHandler
    {

        /// <summary>
        /// Returns <c>true</c> if the handler can submit the specified request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Priority CanSubmit(Request request);

        /// <summary>
        /// Initiates the submission.
        /// </summary>
        /// <param name="elementrequest"></param>
        /// <returns></returns>
        Response Submit(Request request);

    }

    [ContractClassFor(typeof(IRequestHandler))]
    abstract class IRequestHandler_Contract:
        IRequestHandler
    {

        public Priority CanSubmit(Request request)
        {
            Contract.Requires<ArgumentNullException>(request != null);
            throw new NotImplementedException();
        }

        public Response Submit(Request request)
        {
            Contract.Requires<ArgumentNullException>(request != null);
            Contract.Ensures(Contract.Result<Response>() != null);
            throw new NotImplementedException();
        }

    }

}
