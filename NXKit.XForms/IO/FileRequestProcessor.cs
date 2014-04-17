using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Net;

using NXKit.XForms.Serialization;

namespace NXKit.XForms.IO
{

    /// <summary>
    /// Handles submissions of the 'file' scheme.
    /// </summary>
    [Export(typeof(IRequestProcessor))]
    public class FileRequestProcessor :
        WebRequestProcessor
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="serializers"></param>
        /// <param name="deserializers"></param>
        [ImportingConstructor]
        public FileRequestProcessor(
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
            if (submit.ResourceUri.Scheme == Uri.UriSchemeFile)
                return Priority.Default;
            else
                return Priority.Ignore;
        }

        protected override string GetMethod(Request request)
        {
            switch (request.Method.ToLowerInvariant())
            {
                case "get":
                    return WebRequestMethods.File.DownloadFile;
                case "put":
                case "post":
                    return WebRequestMethods.File.UploadFile;
            }

            return null;
        }

        protected override bool IsQuery(Request request)
        {
            return false;
        }

    }

}
