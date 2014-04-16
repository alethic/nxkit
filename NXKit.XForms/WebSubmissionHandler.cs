using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;

using NXKit.Util;

namespace NXKit.XForms
{

    /// <summary>
    /// Handles submissions that can be done using the standard <see cref="WebRequest"/> infrastructure.
    /// </summary>
    [Export(typeof(ISubmissionHandler))]
    public class WebSubmissionHandler :
        ISubmissionHandler
    {

        readonly IEnumerable<ISubmissionSerializer> serializers;
        readonly IEnumerable<ISubmissionDeserializer> deserializers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="serializers"></param>
        /// <param name="deserializers"></param>
        [ImportingConstructor]
        public WebSubmissionHandler(
            [ImportMany] IEnumerable<ISubmissionSerializer> serializers,
            [ImportMany] IEnumerable<ISubmissionDeserializer> deserializers)
        {
            Contract.Requires<ArgumentNullException>(serializers != null);
            Contract.Requires<ArgumentNullException>(deserializers != null);

            this.serializers = serializers;
            this.deserializers = deserializers;
        }

        /// <summary>
        /// Gets the appropriate web method type for the given <see cref="SubmissionRequest"/>.
        /// </summary>
        /// <param name="submit"></param>
        /// <returns></returns>
        string GetMethod(SubmissionRequest submit)
        {
            switch (submit.Method.ToLowerInvariant())
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
        /// Gets the serialization <see cref="MediaType"/> to be used on outgoing data.
        /// </summary>
        /// <param name="submit"></param>
        /// <returns></returns>
        MediaRange GetMediaType(SubmissionRequest submit)
        {
            switch (submit.Method)
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
        /// Gets the serializer which supports the given <see cref="MediaRange"/>.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        ISubmissionSerializer GetSerializer(XNode node, MediaRange mediaType)
        {
            Contract.Requires<ArgumentNullException>(node != null);
            Contract.Requires<ArgumentNullException>(mediaType != null);

            return serializers
                .FirstOrDefault(i => i.CanSerialize(node, mediaType));
        }

        /// <summary>
        /// Gets the deserializer which supports the given <see cref="MediaRange"/>.
        /// </summary>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        ISubmissionDeserializer GetDeserializer(MediaRange mediaType)
        {
            Contract.Requires<ArgumentNullException>(mediaType != null);

            return deserializers
                .FirstOrDefault(i => i.CanDeserialize(mediaType));
        }

        /// <summary>
        /// Attempts to retrieve a <see cref="WebRequest"/> for the given <see cref="SubmissionRequest"/>.
        /// </summary>
        /// <param name="submit"></param>
        /// <returns></returns>
        WebRequest GetWebRequest(SubmissionRequest submit)
        {
            Contract.Requires<ArgumentNullException>(submit != null);

            var method = GetMethod(submit);
            if (method == null)
                return null;

            var serialization = GetMediaType(submit);
            if (serialization == null)
                return null;

            var request = (WebRequest)submit.Context.WebRequest;
            if (request != null)
                return request;

            try
            {
                // store request on context so we don't have to create it twice
                request = submit.Context.WebRequest = WebRequest.Create(submit.ResourceUri);
                request.Method = method;
                return request;
            }
            catch (NotSupportedException)
            {
                return null;
            }
        }

        public bool CanSubmit(SubmissionRequest submit)
        {
            return GetWebRequest(submit) != null;
        }

        /// <summary>
        /// Serializes the <see cref="SubmissionRequest"/> to the <see cref="WebRequest"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="submit"></param>
        void Serialize(WebRequest request, SubmissionRequest submit)
        {
            Contract.Requires<ArgumentNullException>(request != null);
            Contract.Requires<ArgumentNullException>(submit != null);

            // populate headers
            foreach (var pair in submit.Headers)
                foreach (var value in pair.Value)
                    request.Headers.Add(pair.Key, value);

            // obtain serializer
            var serializer = GetSerializer(submit.Body, GetMediaType(submit));
            if (serializer == null)
                throw new InvalidOperationException();

            if (submit.Body != null)
                using (var wrt = new StreamWriter(request.GetRequestStream(), submit.Encoding))
                    serializer.Serialize(wrt, submit.Body);

            request.ContentType = GetMediaType(submit);
        }

        /// <summary>
        /// Gets the <see cref="SubmissionStatus"/> for the response.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        SubmissionStatus GetStatus(WebResponse response)
        {
            if (response is HttpWebResponse)
                return ((HttpWebResponse)response).StatusCode == HttpStatusCode.OK ? SubmissionStatus.Success : SubmissionStatus.Error;
            else
                return SubmissionStatus.Success;
        }

        /// <summary>
        /// Deserializes the <see cref="WebResponse"/> into a <see cref="SubmissionResponse"/>.
        /// </summary>
        /// <param name="web"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        SubmissionResponse Deserialize(WebResponse web, SubmissionRequest request)
        {
            Contract.Requires<ArgumentNullException>(web != null);
            Contract.Requires<ArgumentNullException>(request != null);

            // obtain serializer
            var deserializer = GetDeserializer(web.ContentType);
            if (deserializer == null)
                throw new InvalidOperationException();

            // generate new response
            var response = new SubmissionResponse(
                request,
                GetStatus(web),
                deserializer.Deserialize(new StreamReader(web.GetResponseStream())));

            // populate headers
            foreach (var name in web.Headers.AllKeys)
                response.Headers.Add(name, web.Headers[name]);

            return response;
        }

        /// <summary>
        /// Submits the request and returns the response.
        /// </summary>
        /// <param name="submit"></param>
        /// <returns></returns>
        public SubmissionResponse Submit(SubmissionRequest submit)
        {
            // obtain request
            var request = GetWebRequest(submit);
            if (request == null)
                throw new InvalidOperationException();

            Serialize(request, submit);

            // submit request and retrieve response
            var response = request.GetResponse();
            if (response == null)
                throw new InvalidOperationException();

            return Deserialize(response, submit);
        }

    }

}
