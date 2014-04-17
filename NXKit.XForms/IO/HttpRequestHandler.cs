using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Net;

using NXKit.XForms.Serialization;

namespace NXKit.XForms.IO
{

    /// <summary>
    /// Handles submissions of the default HTTP scheme's expressed by the XForms standard.
    /// </summary>
    [Export(typeof(IRequestHandler))]
    public class HttpRequestHandler :
        WebRequestHandler
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="serializers"></param>
        /// <param name="deserializers"></param>
        [ImportingConstructor]
        public HttpRequestHandler(
            [ImportMany] IEnumerable<INodeSerializer> serializers,
            [ImportMany] IEnumerable<INodeDeserializer> deserializers)
            : base(serializers, deserializers)
        {
            Contract.Requires<ArgumentNullException>(serializers != null);
            Contract.Requires<ArgumentNullException>(deserializers != null);
        }

        /// <summary>
        /// Returns <c>true</c> if the processor can handle this request.
        /// </summary>
        /// <param name="submit"></param>
        /// <returns></returns>
        public override Priority CanSubmit(Request submit)
        {
            if (submit.ResourceUri.Scheme == Uri.UriSchemeHttp ||
                submit.ResourceUri.Scheme == Uri.UriSchemeHttps)
                return Priority.Default;
            else
                return Priority.Ignore;
        }

        protected override ResponseStatus ReadRequestStatus(WebResponse response)
        {
            var webResponse = response as HttpWebResponse;
            if (webResponse == null)
                throw new InvalidOperationException();

            // success ranges for HTTP
            if ((int)webResponse.StatusCode >= 200 &&
                (int)webResponse.StatusCode <= 299)
                return ResponseStatus.Success;
            else
                return ResponseStatus.Error;
        }

    }

}
