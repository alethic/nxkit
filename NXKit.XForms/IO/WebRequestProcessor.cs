using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net;
using System.Text;

using NXKit.Util;
using NXKit.XForms.Serialization;

namespace NXKit.XForms.IO
{

    /// <summary>
    /// Base <see cref="IRequestProcessor"/> implementation for using the Web request framework.
    /// </summary>
    [Export(typeof(IRequestProcessor))]
    public class WebRequestProcessor :
        RequestProcessor
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="serializers"></param>
        /// <param name="deserializers"></param>
        [ImportingConstructor]
        public WebRequestProcessor(
            [ImportMany] IEnumerable<INodeSerializer> serializers,
            [ImportMany] IEnumerable<INodeDeserializer> deserializers)
            : base(serializers, deserializers)
        {
            Contract.Requires<ArgumentNullException>(serializers != null);
            Contract.Requires<ArgumentNullException>(deserializers != null);
        }

        public override Priority CanSubmit(Request request)
        {
            return Priority.Low;
        }

        /// <summary>
        /// Gets the <see cref="MediaType"/> to be used on outgoing data.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected override MediaRange GetMediaType(Request request)
        {
            switch (request.Method)
            {
                case "post":
                case "put":
                    return "application/xml";
                case "get":
                case "delete":
                case "urlencoded-post":
                    return "application/x-www-form-urlencoded";
                case "multipart-post":
                    return "multipart/related";
                case "form-data-post":
                    return "multipart/form-data";
            }

            return null;
        }

        /// <summary>
        /// Gets the appropriate Web Request method type for the given <see cref="Request"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected virtual string GetMethod(Request request)
        {
            Contract.Requires<ArgumentNullException>(request != null);

            switch (request.Method.ToLowerInvariant())
            {
                case "get":
                    return "GET";
                case "put":
                    return "PUT";
                case "post":
                    return "POST";
                case "delete":
                    return "DELETE";
                default:
                    return null;
            }
        }

        /// <summary>
        /// Returns <c>true</c> if the request should append serialized data to the URI query path.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected virtual bool IsQuery(Request request)
        {
            // get http method
            var method = GetMethod(request);
            if (method == null)
                throw new InvalidOperationException();

            return
                method == "GET" ||
                method == "DELETE";
        }

        /// <summary>
        /// Retrieves a <see cref="WebRequest"/> for the given <see cref="Request"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected virtual WebRequest WriteWebRequest(Request request)
        {
            Contract.Requires<ArgumentNullException>(request != null);

            var mediaType = GetMediaType(request);
            if (mediaType == null)
                return null;

            // obtain and build request parts
            var cnt = (string)null;
            var stm = new MemoryStream();
            var uri = request.ResourceUri;
            if (IsQuery(request))
            {
                // serialize body to string
                var u = new UriBuilder(uri);
                var s = new StringBuilder(u.Query);
                using (var w = new StringWriter(s))
                    Serialize(w, request.Body, mediaType);
                u.Query = s.ToString();

                // replace uri
                uri = u.Uri;
            }
            else
            {
                // serialize data to body
                using (var w = new StreamWriter(stm, request.Encoding, 1024, true))
                    Serialize(w, request.Body, mediaType);

                // configure body
                cnt = mediaType;
                stm.Position = 0;
            }

            // generate new web request
            var webRequest = WebRequest.Create(uri);
            webRequest.Method = GetMethod(request);
            webRequest.ContentType = cnt;

            // populate headers
            foreach (var pair in request.Headers)
                foreach (var value in pair.Value)
                    webRequest.Headers.Add(pair.Key, value);

            // write body to stream, if available
            if (stm.Length > 0)
                using (var req = webRequest.GetRequestStream())
                    stm.CopyTo(req);

            return webRequest;
        }

        /// <summary>
        /// Gets the <see cref="ResponseStatus"/> for the response.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        protected virtual ResponseStatus ReadRequestStatus(WebResponse response)
        {
            return ResponseStatus.Error;
        }

        /// <summary>
        /// Deserializes the <see cref="WebResponse"/> into a <see cref="Response"/>.
        /// </summary>
        /// <param name="webResponse"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        protected virtual Response ReadWebResponse(WebResponse webResponse, Request request)
        {
            Contract.Requires<ArgumentNullException>(webResponse != null);
            Contract.Requires<ArgumentNullException>(request != null);

            // obtain serializer
            var deserializer = GetDeserializer(webResponse.ContentType);
            if (deserializer == null)
                throw new InvalidOperationException();

            // generate new response
            var response = new Response(
                request,
                ReadRequestStatus(webResponse),
                deserializer.Deserialize(
                    new StreamReader(webResponse.GetResponseStream()),
                    webResponse.ContentType));

            // populate headers
            foreach (var name in webResponse.Headers.AllKeys)
                response.Headers.Add(name, webResponse.Headers[name]);

            return response;
        }

        /// <summary>
        /// Submits the request and returns the response.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public override Response Submit(Request request)
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
            var response = ReadWebResponse(webResponse, request);
            if (response == null)
                throw new InvalidOperationException();

            return response;
        }

    }

}
