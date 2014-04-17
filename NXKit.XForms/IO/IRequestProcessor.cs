using System;
using System.Diagnostics.Contracts;

namespace NXKit.XForms.IO
{

    /// <summary>
    /// Defines a class capable of handling a submission.
    /// </summary>
    [ContractClass(typeof(ISubmissionProcessor_Contract))]
    public interface IRequestProcessor
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

    [ContractClassFor(typeof(IRequestProcessor))]
    abstract class ISubmissionProcessor_Contract:
        IRequestProcessor
    {

        public Priority CanSubmit(Request request)
        {
            Contract.Requires<ArgumentNullException>(request != null);
            throw new NotImplementedException();
        }

        public Response Submit(Request request)
        {
            Contract.Requires<ArgumentNullException>(request != null);
            throw new NotImplementedException();
        }

    }

}
