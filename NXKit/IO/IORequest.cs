using System;
using System.Diagnostics;
using System.IO;

using NXKit.IO.Media;
using NXKit.Util;

namespace NXKit.IO
{

    /// <summary>
    /// Describes an IO request to be dispatched towards a resource in order to obtain a response.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class IORequest
    {

        readonly Uri resourceUri;
        readonly Headers headers;
        readonly DynamicDictionary context;
        IOMethod method;
        Stream content;
        MediaRange contentType;
        MediaRangeList accept;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resourceUri"></param>
        /// <param name="method"></param>
        public IORequest(Uri resourceUri, IOMethod method)
        {
            if (resourceUri == null)
                throw new ArgumentNullException(nameof(resourceUri));
            if (resourceUri.IsAbsoluteUri == false)
                throw new ArgumentOutOfRangeException(nameof(resourceUri));

            this.resourceUri = resourceUri;
            this.method = method;
            this.headers = new Headers();
            this.context = new DynamicDictionary();
        }

        /// <summary>
        /// Gets the <see cref="Uri"/> of the resource being requested.
        /// </summary>
        public Uri ResourceUri
        {
            get { return resourceUri; }
        }

        /// <summary>
        /// Gets or sets the method type of the request.
        /// </summary>
        public virtual IOMethod Method
        {
            get { return method; }
            set { method = value; }
        }

        /// <summary>
        /// Gets or sets the body to be sent along with the request.
        /// </summary>
        public virtual Stream Content
        {
            get { return content; }
            set { content = value; }
        }

        /// <summary>
        /// Gets or sets the preferred media type in which to submit the request.
        /// </summary>
        public virtual MediaRange ContentType
        {
            get { return contentType; }
            set { contentType = value; }
        }

        /// <summary>
        /// Gets or sets the set of media ranges which should be accepted in a response.
        /// </summary>
        public virtual MediaRangeList Accept
        {
            get { return accept; }
            set { accept = value; }
        }

        /// <summary>
        /// Gets the set of additional headers to be provided along with the request.
        /// </summary>
        public Headers Headers
        {
            get { return headers; }
        }

        /// <summary>
        /// Gets a dynamic object which can be used to attach other information to the request.
        /// </summary>
        public dynamic Context
        {
            get { return context; }
        }

        string DebuggerDisplay
        {
            get { return string.Format("{0} {1}", method, resourceUri); }
        }

    }

}
