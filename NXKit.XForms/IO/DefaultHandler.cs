using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net;
using System.Text;
using NXKit.IO;
using NXKit.Util;
using NXKit.XForms.Serialization;

namespace NXKit.XForms.IO
{

    /// <summary>
    /// Base <see cref="IModelRequestHandler"/> implementation for using the Web request framework.
    /// </summary>
    [Export(typeof(IModelRequestHandler))]
    public class DefaultHandler :
        ModelRequestHandler
    {

        readonly IIOService ioService;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="ioService"></param>
        /// <param name="serializers"></param>
        /// <param name="deserializers"></param>
        [ImportingConstructor]
        public DefaultHandler(
            IIOService ioService,
            [ImportMany] IEnumerable<IModelSerializer> serializers,
            [ImportMany] IEnumerable<IModelDeserializer> deserializers)
            : base(serializers, deserializers)
        {
            Contract.Requires<ArgumentNullException>(ioService != null);
            Contract.Requires<ArgumentNullException>(serializers != null);
            Contract.Requires<ArgumentNullException>(deserializers != null);

            this.ioService = ioService;
        }

        public override Priority CanSubmit(ModelRequest request)
        {
            return Priority.Low;
        }

        /// <summary>
        /// Gets the <see cref="MediaType"/> to be used on outgoing data.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected override MediaRange GetMediaType(ModelRequest request)
        {
            if (request.Method == ModelMethod.Post ||
                request.Method == ModelMethod.Put)
                return "application/xml";

            if (request.Method == ModelMethod.Get ||
                request.Method == ModelMethod.Delete ||
                request.Method == ModelMethod.UrlEncodedPost)
                return "application/x-www-form-urlencoded";

            if (request.Method == ModelMethod.MultipartPost)
                return "multipart/related";

            if (request.Method == ModelMethod.FormDataPost)
                return "multipart/form-data";

            return null;
        }

        /// <summary>
        /// Gets the appropriate Web Request method type for the given <see cref="ModelRequest"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected virtual IOMethod GetMethod(ModelRequest request)
        {
            Contract.Requires<ArgumentNullException>(request != null);

            if (request.Method == ModelMethod.Get)
                return IOMethod.Get;

            if (request.Method == ModelMethod.Put)
                return IOMethod.Put;

            if (request.Method == ModelMethod.Post ||
                request.Method == ModelMethod.UrlEncodedPost ||
                request.Method == ModelMethod.MultipartPost ||
                request.Method == ModelMethod.FormDataPost)
                return IOMethod.Post;

            if (request.Method == ModelMethod.Delete)
                return IOMethod.Delete;

            return IOMethod.None;
        }

        /// <summary>
        /// Returns <c>true</c> if the request should append serialized data to the URI query path.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected virtual bool IsQuery(ModelRequest request)
        {
            Contract.Requires<ArgumentNullException>(request != null);

            // get http method
            var method = GetMethod(request);
            if (method == null)
                throw new InvalidOperationException();

            return
                method == IOMethod.Get ||
                method == IOMethod.Delete;
        }

        /// <summary>
        /// Retrieves a <see cref="WebRequest"/> for the given <see cref="ModelRequest"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected virtual IORequest WriteIORequest(ModelRequest request)
        {
            Contract.Requires<ArgumentNullException>(request != null);

            var mediaType = GetMediaType(request);
            if (mediaType == null)
                return null;

            // obtain and build request parts
            var cnt = (string)null;
            var stm = new MemoryStream();
            var uri = request.ResourceUri;
            if (IsQuery(request) && request.Body != null)
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
            else if (request.Body != null)
            {
                // serialize data to body
                using (var w = new StreamWriter(stm, request.Encoding, 1024, true))
                    Serialize(w, request.Body, mediaType);

                // configure body
                cnt = mediaType;
                stm.Position = 0;
            }

            // generate new web request
            var ioRequest = new IORequest(uri, GetMethod(request));
            ioRequest.ContentType = cnt;
            ioRequest.Headers.Add(request.Headers);
            ioRequest.Content = stm;

            return ioRequest;
        }

        /// <summary>
        /// Gets the <see cref="ModelResponseStatus"/> for the response.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        protected virtual ModelResponseStatus ReadRequestStatus(IOResponse response)
        {
            Contract.Requires<ArgumentNullException>(response != null);

            return response.Status == IOStatus.Success ? ModelResponseStatus.Success : ModelResponseStatus.Error;
        }

        /// <summary>
        /// Extracts an <see cref="ModelResponse"/> from the given <see cref="IOResponse"/> content.
        /// </summary>
        /// <param name="ioResponse"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        ModelResponse ReadIOResponseFromContent(IOResponse ioResponse, ModelRequest request)
        {
            Contract.Requires<ArgumentNullException>(ioResponse != null);
            Contract.Requires<ArgumentNullException>(request != null);

            // deserialize if possible
            if (ioResponse.Content != null &&
                ioResponse.ContentType != null)
            {
                // obtain serializer
                var deserializer = GetDeserializer(ioResponse.ContentType);
                if (deserializer == null)
                    throw new UnsupportedMediaTypeException();

                // generate new response
                return new ModelResponse(
                    request,
                    ReadRequestStatus(ioResponse),
                    deserializer.Deserialize(
                        new StreamReader(ioResponse.Content),
                        ioResponse.ContentType));
            }
            else
            {
                return new ModelResponse(
                    request,
                    ReadRequestStatus(ioResponse),
                    null);
            }
        }

        /// <summary>
        /// Deserializes the <see cref="WebResponse"/> into a <see cref="ModelResponse"/>.
        /// </summary>
        /// <param name="ioResponse"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        protected virtual ModelResponse ReadIOResponse(IOResponse ioResponse, ModelRequest request)
        {
            Contract.Requires<ArgumentNullException>(ioResponse != null);
            Contract.Requires<ArgumentNullException>(request != null);

            var response = ReadIOResponseFromContent(ioResponse, request);
            response.Headers.Add(ioResponse.Headers);
            return response;
        }

        /// <summary>
        /// Submits the request and returns the response.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public override ModelResponse Submit(ModelRequest request)
        {
            // write request
            var ioRequest = WriteIORequest(request);
            if (ioRequest == null)
                throw new InvalidOperationException();

            // submit request and retrieve response
            var ioResponse = ioService.Send(ioRequest);
            if (ioResponse == null)
                throw new InvalidOperationException();

            // read response
            var response = ReadIOResponse(ioResponse, request);
            if (response == null)
                throw new InvalidOperationException();

            return response;
        }

    }

}
