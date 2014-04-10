using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net;

namespace NXKit.Net
{

    /// <summary>
    /// Describes a dynamic web request.
    /// </summary>
    public class DynamicWebRequest :
         WebRequest
    {

        Uri uri;
        string method;
        long contentLength;
        string contentType;
        readonly WebHeaderCollection headers;
        readonly Stream stream;
        Lazy<WebResponse> response;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="uri"></param>
        public DynamicWebRequest(Uri uri)
            : base()
        {
            Contract.Requires<ArgumentNullException>(uri != null);
            Contract.Requires<ArgumentOutOfRangeException>(uri.Scheme == DynamicUriHelper.UriSchemeDynamic);

            this.uri = uri;
            this.method = "GET";
            this.contentLength = 0;
            this.headers = new WebHeaderCollection();
            this.stream = new MemoryStream();

            this.response = new Lazy<WebResponse>(() => DynamicUriRegistry.Get(new Guid(RequestUri.Authority)).GetResponse(this));
        }

        public override Uri RequestUri
        {
            get { return uri; }
        }

        public override string Method
        {
            get { return method; }
            set { method = value; }
        }

        public override long ContentLength
        {
            get { return contentLength; }
            set { contentLength = value; }
        }

        public override string ContentType
        {
            get { return contentType; }
            set { contentType = value; }
        }

        public override WebHeaderCollection Headers
        {
            get { return headers; }
        }

        public override Stream GetRequestStream()
        {
            return stream;
        }

        public override WebResponse GetResponse()
        {
            return response.Value;
        }

    }

}
