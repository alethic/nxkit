using System;
using System.Diagnostics.Contracts;

namespace NXKit.XForms.IO
{

    /// <summary>
    /// Defines a class capable of handling a submission.
    /// </summary>
    [ContractClass(typeof(ISubmissionProcessor_Contract))]
    public interface ISubmissionProcessor
    {

        /// <summary>
        /// Returns <c>true</c> if the handler can submit the specified request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Priority CanSubmit(SubmissionRequest request);

        /// <summary>
        /// Initiates the submission.
        /// </summary>
        /// <param name="elementrequest"></param>
        /// <returns></returns>
        SubmissionResponse Submit(SubmissionRequest request);

    }

    [ContractClassFor(typeof(ISubmissionProcessor))]
    abstract class ISubmissionProcessor_Contract:
        ISubmissionProcessor
    {

        public Priority CanSubmit(SubmissionRequest request)
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
