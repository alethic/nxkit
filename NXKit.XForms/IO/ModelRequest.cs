using System;
using System.Diagnostics.Contracts;
using System.Text;
using System.Xml.Linq;

using NXKit.IO;
using NXKit.IO.Media;
using NXKit.Util;

namespace NXKit.XForms.IO
{

    /// <summary>
    /// Describes an IO request to be dispatched towards a resource.
    /// </summary>
    public class ModelRequest
    {

        readonly Uri resourceUri;
        readonly Headers headers;
        readonly DynamicDictionary context;
        ModelMethod method;
        MediaRange mediaType;
        XNode body;
        Encoding encoding;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resourceUri"></param>
        /// <param name="method"></param>
        public ModelRequest(
            Uri resourceUri,
            ModelMethod method)
        {
            Contract.Requires<ArgumentNullException>(resourceUri != null);
            Contract.Requires<ArgumentException>(resourceUri.IsAbsoluteUri);

            this.resourceUri = resourceUri;
            this.method = method;
            this.encoding = Encoding.UTF8;
            this.headers = new Headers();
            this.context = new DynamicDictionary();
        }

        /// <summary>
        /// Gets the <see cref="Uri"/> of the resource being requested.
        /// </summary>
        public Uri ResourceUri
        {
            get { Contract.Ensures(Contract.Result<Uri>() != null); return resourceUri; }
        }

        /// <summary>
        /// Gets or sets the method type of the request.
        /// </summary>
        public ModelMethod Method
        {
            get { return method; }
            set { method = value; }
        }

        /// <summary>
        /// Gets or sets the body to be sent along with the request.
        /// </summary>
        public XNode Body
        {
            get { return body; }
            set { body = value; }
        }

        /// <summary>
        /// Gets or sets the preferred media type in which to submit the request.
        /// </summary>
        public MediaRange MediaType
        {
            get { return mediaType; }
            set { mediaType = value; }
        }

        /// <summary>
        /// Gets or sets the encoding in which to submit the request.
        /// </summary>
        public Encoding Encoding
        {
            get { Contract.Ensures(Contract.Result<Encoding>() != null); return encoding; }
            set { Contract.Requires<ArgumentException>(value != null); encoding = value; }
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

    }

}
