using System;
using System.IO;

namespace NXKit.Web.UI
{

    public class ResourceActionEventArgs : 
        EventArgs
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="uri"></param>
        internal ResourceActionEventArgs(ResourceActionMethod method, Uri uri)
        {
            Method = method;
            Uri = uri;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="uri"></param>
        /// <param name="body"></param>
        internal ResourceActionEventArgs(ResourceActionMethod method, Uri uri, Stream body)
            : this(method, uri)
        {
            Body = body;
        }

        public ResourceActionMethod Method { get; private set; }

        public Uri Uri { get; private set; }

        public Stream Body { get; private set; }

        public Stream Result { get; set; }

        public string ReferenceUri { get; set; }

    }

}
