using System;
using System.Diagnostics.Contracts;

namespace NXKit.XForms
{

    /// <summary>
    /// Defines a class capable of handling a submission.
    /// </summary>
    [ContractClass(typeof(ISubmissionHandler_Contract))]
    public interface ISubmissionHandler
    {

        /// <summary>
        /// Returns <c>true</c> if the handler can submit the specified request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        bool CanSubmit(SubmissionRequest request);

        /// <summary>
        /// Initiates the submission.
        /// </summary>
        /// <param name="elementrequest"></param>
        /// <returns></returns>
        SubmissionResponse Submit(SubmissionRequest request);

    }

    [ContractClassFor(typeof(ISubmissionHandler))]
    abstract class ISubmissionHandler_Contract:
        ISubmissionHandler
    {

        public bool CanSubmit(SubmissionRequest request)
        {
            Contract.Requires<ArgumentNullException>(request != null);
            throw new NotImplementedException();
        }

        public SubmissionResponse Submit(SubmissionRequest request)
        {
            Contract.Requires<ArgumentNullException>(request != null);
            throw new NotImplementedException();
        }

    }

}
