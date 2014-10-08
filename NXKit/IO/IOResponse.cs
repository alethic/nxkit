using System;
using System.Diagnostics.Contracts;
using System.IO;

using NXKit.IO.Media;

namespace NXKit.IO
{

    /// <summary>
    /// Describes a response of a submission returned from a <see cref="IIOTransport"/>.
    /// </summary>
    public class IOResponse
    {

        readonly IORequest request;
        Headers headers;
        IOStatus status;
        Stream content;
        MediaRange contentType;

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
            set { status = value; }
        }

        /// <summary>
        /// Gets the resulting response body.
        /// </summary>
        public Stream Content
        {
            get { return content; }
            set { content = value; }
        }

        /// <summary>
        /// Gets the resulting content type.
        /// </summary>
        public MediaRange ContentType
        {
            get { return contentType; }
            set { contentType = value; }
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
