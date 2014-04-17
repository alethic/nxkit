using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.XForms.IO
{

    /// <summary>
    /// Describes a response of a submission returned from a <see cref="ISubmissionProcessor"/>.
    /// </summary>
    public class Response
    {

        readonly Request request;
        readonly SubmissionStatus status;
        readonly XObject body;
        readonly SubmissionHeaders headers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="request"></param>
        public Response(Request request, SubmissionStatus status, XObject body)
        {
            Contract.Requires<ArgumentNullException>(request != null);

            this.request = request;
            this.status = status;
            this.body = body;
            this.headers = new SubmissionHeaders();
        }

        /// <summary>
        /// Gets the original submitted request that resulted in this response.
        /// </summary>
        public Request Request
        {
            get { return request; }
        }

        /// <summary>
        /// Gets the resulting status.
        /// </summary>
        public SubmissionStatus Status
        {
            get { return status; }
        }

        /// <summary>
        /// Gets the resulting response body.
        /// </summary>
        public XObject Body
        {
            get { return body; }
        }

        public SubmissionHeaders Headers
        {
            get { return headers; }
        }

    }

}
