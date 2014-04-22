using System;
using System.ComponentModel.Composition;
using System.Net;

namespace NXKit.IO
{

    /// <summary>
    /// Handles submissions of the default HTTP scheme's expressed by the XForms standard.
    /// </summary>
    [Export(typeof(IIOTransport))]
    public class HttpTransport :
        WebTransport
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        [ImportingConstructor]
        public HttpTransport()
            : base()
        {

        }

        /// <summary>
        /// Returns <c>true</c> if the processor can handle this request.
        /// </summary>
        /// <param name="submit"></param>
        /// <returns></returns>
        public override Priority CanSend(IORequest submit)
        {
            if (submit.ResourceUri.Scheme == Uri.UriSchemeHttp ||
                submit.ResourceUri.Scheme == Uri.UriSchemeHttps)
                return Priority.Default;
            else
                return Priority.Ignore;
        }

        protected override IOStatus ReadResponseStatus(WebResponse webResponse)
        {
            var httpResponse = webResponse as HttpWebResponse;
            if (httpResponse == null)
                throw new InvalidOperationException();

            if ((int)httpResponse.StatusCode >= 200 &&
                (int)httpResponse.StatusCode <= 299)
                return IOStatus.Success;

            if ((int)httpResponse.StatusCode >= 300 &&
                (int)httpResponse.StatusCode <= 399)
                return IOStatus.Redirect;

            if ((int)httpResponse.StatusCode >= 400 &&
                (int)httpResponse.StatusCode <= 499)
                return IOStatus.ClientError;

            if ((int)httpResponse.StatusCode >= 500 &&
                (int)httpResponse.StatusCode <= 599)
                return IOStatus.ServerError;

            return IOStatus.Unknown;
        }

    }

}
