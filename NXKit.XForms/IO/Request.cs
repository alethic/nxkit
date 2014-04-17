using System;
using System.Diagnostics.Contracts;
using System.Text;
using System.Xml.Linq;

using NXKit.Util;

namespace NXKit.XForms.IO
{

    /// <summary>
    /// Describes a submission for forwarding to <see cref="ISubmissionProcessor"/>s.
    /// </summary>
    public class Request
    {

        readonly Uri resourceUri;
        readonly string method;
        readonly SubmissionSerialization serialization;
        readonly MediaRange mediaType;
        readonly XNode body;
        readonly Encoding encoding;
        readonly SubmissionHeaders headers;
        readonly DynamicDictionary context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resourceUri"></param>
        /// <param name="method"></param>
        /// <param name="serialization"></param>
        /// <param name="mediaType"></param>
        /// <param name="body"></param>
        public Request(
            Uri resourceUri,
            string method,
            SubmissionSerialization serialization,
            MediaRange mediaType,
            XNode body,
            Encoding encoding,
            SubmissionHeaders headers)
        {
            Contract.Requires<ArgumentNullException>(resourceUri != null);
            Contract.Requires<ArgumentException>(resourceUri.IsAbsoluteUri);
            Contract.Requires<ArgumentNullException>(encoding != null);
            Contract.Requires<ArgumentNullException>(headers != null);

            this.resourceUri = resourceUri;
            this.method = method;
            this.mediaType = mediaType;
            this.body = body;
            this.encoding = encoding;
            this.headers = headers;
            this.context = new DynamicDictionary();
        }

        public Uri ResourceUri
        {
            get { return resourceUri; }
        }

        public string Method
        {
            get { return method; }
        }

        public SubmissionSerialization Serialization
        {
            get { return serialization; }
        }

        public MediaRange MediaType
        {
            get { return mediaType; }
        }

        public XNode Body
        {
            get { return body; }
        }

        public Encoding Encoding
        {
            get { return encoding; }
        }

        public SubmissionHeaders Headers
        {
            get { return headers; }
        }

        public dynamic Context
        {
            get { return context; }
        }

    }

}
