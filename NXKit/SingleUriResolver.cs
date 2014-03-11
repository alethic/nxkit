using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace NXKit
{

    /// <summary>
    /// Provides a resolver that handles a single relative path.
    /// </summary>
    public class SingleUriResolver :
        IResolver
    {

        readonly Uri uri;
        readonly Func<Stream> getFunc;
        readonly DefaultResolver source;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="getFunc"></param>
        public SingleUriResolver(string uri, Func<Stream> getFunc)
            : this(new Uri(uri, UriKind.Relative), getFunc)
        {
            Contract.Requires<ArgumentNullException>(uri != null);
            Contract.Requires<ArgumentNullException>(getFunc != null);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="getFunc"></param>
        public SingleUriResolver(Uri uri, Func<Stream> getFunc)
            : base()
        {
            Contract.Requires<ArgumentNullException>(uri != null);
            Contract.Requires<ArgumentNullException>(getFunc != null);

            this.uri = uri;
            this.getFunc = getFunc;
            this.source = new DefaultResolver();
        }

        public Stream Get(Uri uri)
        {
            return uri == this.uri ? getFunc() : source.Get(uri);
        }

        public Stream Put(Uri uri, Stream stream)
        {
            return source.Put(uri, stream);
        }

    }

}
