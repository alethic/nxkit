using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.XForms.IO
{

    /// <summary>
    /// Describes a response of a submission returned from a <see cref="IRequestProcessor"/>.
    /// </summary>
    public class Response
    {

        readonly Request request;
        readonly ResponseStatus status;
        readonly XObject body;
        readonly Headers headers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="request"></param>
        public Response(Request request, ResponseStatus status, XObject body)
        {
            Contract.Requires<ArgumentNullException>(request != null);

            this.request = request;
            this.status = status;
            this.body = body;
            this.headers = new Headers();
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
        public ResponseStatus Status
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

        public Headers Headers
        {
            get { return headers; }
        }

    }

}
