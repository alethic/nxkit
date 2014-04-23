using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;

using NXKit.IO;
using NXKit.XForms.Serialization;

namespace NXKit.XForms.IO
{

    /// <summary>
    /// Handles submissions of the default HTTP scheme's expressed by the XForms standard.
    /// </summary>
    [Export(typeof(IModelRequestHandler))]
    public class HttpHandler :
        DefaultHandler
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="ioService"></param>
        /// <param name="serializers"></param>
        /// <param name="deserializers"></param>
        [ImportingConstructor]
        public HttpHandler(
            IIOService ioService,
            [ImportMany] IEnumerable<IModelSerializer> serializers,
            [ImportMany] IEnumerable<IModelDeserializer> deserializers)
            : base(ioService, serializers, deserializers)
        {
            Contract.Requires<ArgumentNullException>(ioService != null);
            Contract.Requires<ArgumentNullException>(serializers != null);
            Contract.Requires<ArgumentNullException>(deserializers != null);
        }

        /// <summary>
        /// Returns <c>true</c> if the processor can handle this request.
        /// </summary>
        /// <param name="submit"></param>
        /// <returns></returns>
        public override Priority CanSubmit(ModelRequest submit)
        {
            if (submit.ResourceUri.Scheme == Uri.UriSchemeHttp ||
                submit.ResourceUri.Scheme == Uri.UriSchemeHttps)
                return Priority.Default;
            else
                return Priority.Ignore;
        }

    }

}
