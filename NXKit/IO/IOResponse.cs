using System;
using System.Diagnostics.Contracts;
using System.IO;

using NXKit.Util;

namespace NXKit.IO
{

    /// <summary>
    /// Describes a response of a submission returned from a <see cref="IIOTransport"/>.
    /// </summary>
    public class IOResponse
    {

        readonly IORequest request;
        readonly Stream content;
        readonly MediaRange contentType;
        readonly Headers headers;
        readonly IOStatus status;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="status"></param>
        public IOResponse(IORequest request, IOStatus status)
        {
            Contract.Requires<ArgumentNullException>(request != null);

            this.request = request;
            this.status = status;
            this.headers = new Headers();
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="status"></param>
        /// <param name="content"></param>
        /// <param name="contentType"></param>
        public IOResponse(IORequest request, IOStatus status, Stream content, MediaRange contentType)
            : this(request, status)
        {
            Contract.Requires<ArgumentNullException>(request != null);
            Contract.Requires<ArgumentNullException>(content != null);

            this.content = content;
            this.contentType = contentType;
        }

        /// <summary>
        /// Gets the original submitted request that resulted in this response.
        /// </summary>
        public IORequest Request
        {
            get { return request; }
        }

        /// <summary>
        /// Gets the status of the response.
        /// </summary>
        public IOStatus Status
        {
            get { return status; }
        }

        /// <summary>
        /// Gets the resulting response body.
        /// </summary>
        public Stream Content
        {
            get { Contract.Ensures(Contract.Result<Stream>() != null); return content; }
        }

        /// <summary>
        /// Gets the resulting content type.
        /// </summary>
        public MediaRange ContentType
        {
            get { return contentType; }
        }

        /// <summary>
        /// Gets the set of headers returned from the request.
        /// </summary>
        public Headers Headers
        {
            get { return headers; }
        }

    }

}
