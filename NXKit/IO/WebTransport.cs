using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Net;

namespace NXKit.IO
{

    /// <summary>
    /// Base <see cref="IIOTransport"/> implementation for using the Web request framework.
    /// </summary>
    [Export(typeof(IIOTransport))]
    public class WebTransport :
        IOTransport
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        [ImportingConstructor]
        public WebTransport()
            : base()
        {

        }

        public override Priority CanSend(IORequest request)
        {
            return Priority.Low;
        }

        /// <summary>
        /// Retrieves a <see cref="WebRequest"/> for the given <see cref="IORequest"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected virtual WebRequest WriteWebRequest(IORequest request)
        {
            Contract.Requires<ArgumentNullException>(request != null);

            // generate new web request
            var webRequest = WebRequest.Create(request.ResourceUri);
            webRequest.Method = request.Method;
            webRequest.ContentType = request.ContentType;

            if (request.Content != null &&
                request.Content.CanRead)
                request.Content.CopyTo(webRequest.GetRequestStream());

            // populate headers
            foreach (var pair in request.Headers)
                webRequest.Headers.Add(pair.Key, pair.Value);

            return webRequest;
        }

        /// <summary>
        /// Deserializes the <see cref="WebResponse"/> into a <see cref="IOResponse"/>.
        /// </summary>
        /// <param name="webResponse"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        protected virtual IOResponse ReadResponse(WebResponse webResponse, IORequest request)
        {
            Contract.Requires<ArgumentNullException>(webResponse != null);
            Contract.Requires<ArgumentNullException>(request != null);

            // generate new response
            var response = new IOResponse(
                request,
                ReadResponseStatus(webResponse),
                webResponse.GetResponseStream(),
                webResponse.ContentType);

            // populate headers
            foreach (var name in webResponse.Headers.AllKeys)
                response.Headers[name] = webResponse.Headers[name];

            return response;
        }

        /// <summary>
        /// Returns the status of the IO request from the <see cref="WebResponse"/>.
        /// </summary>
        /// <param name="webResponse"></param>
        /// <returns></returns>
        protected virtual IOStatus ReadResponseStatus(WebResponse webResponse)
        {
            return IOStatus.Success;
        }

        /// <summary>
        /// Submits the request and returns the response.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public override IOResponse Submit(IORequest request)
        {
            // write request
            var webRequest = WriteWebRequest(request);
            if (webRequest == null)
                throw new InvalidOperationException();

            // submit request and retrieve response
            var webResponse = webRequest.GetResponse();
            if (webResponse == null)
                throw new InvalidOperationException();

            // read response
            var response = ReadResponse(webResponse, request);
            if (response == null)
                throw new InvalidOperationException();

            return response;
        }

    }

}
