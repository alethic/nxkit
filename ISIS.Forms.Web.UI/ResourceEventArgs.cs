using System;
using System.IO;

namespace ISIS.Forms.Web.UI
{

    public class ResourceActionEventArgs : EventArgs
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="baseUri"></param>
        /// <param name="href"></param>
        internal ResourceActionEventArgs(ResourceActionMethod method, string href, string baseUri)
        {
            Method = method;
            Href = href;
            BaseUri = baseUri;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="baseUri"></param>
        /// <param name="href"></param>
        /// <param name="body"></param>
        internal ResourceActionEventArgs(ResourceActionMethod method, string href, string baseUri, Stream body)
            : this(method, href, baseUri)
        {
            Body = body;
        }

        public ResourceActionMethod Method { get; private set; }

        public string Href { get; private set; }

        public string BaseUri { get; private set; }

        public Stream Body { get; private set; }

        public Stream Result { get; set; }

        public string ReferenceUri { get; set; }

    }

}
