using System;
using System.Xml.Linq;

namespace NXKit
{

    public class SubmitErrorContextInfo
    {

        readonly SubmitErrorErrorType errorType;
        readonly string resourceUri;
        readonly int? responseStatusCode;
        readonly XElement[] responseHeaders;
        readonly string responseReasonPhrase;
        readonly object responseBody;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public SubmitErrorContextInfo(
            SubmitErrorErrorType errorType,
            string resourceUri = null,
            int? responseStatusCode = null,
            XElement[] responseHeaders = null,
            string responseReasonPhrase = "",
            object responseBody = null)
        {
            this.errorType = errorType;
            this.resourceUri = resourceUri;
            this.responseStatusCode = responseStatusCode;
            this.responseHeaders = responseHeaders;
            this.responseReasonPhrase = responseReasonPhrase ?? throw new ArgumentNullException(nameof(responseReasonPhrase));
            this.responseBody = responseBody;
        }

    }

}
