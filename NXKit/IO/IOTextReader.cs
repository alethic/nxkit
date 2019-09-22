using System;
using System.IO;
using System.Text;

using NXKit.IO.Media;

namespace NXKit.IO
{

    public class IOTextReader :
        StreamReader
    {

        static readonly MediaRangeList TXT_MEDIA_RANGES = new[] {
            "text/plain",
        };

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="ioService"></param>
        /// <param name="uri"></param>
        /// <param name="accept"></param>
        public static TextReader Create(IIOService ioService, Uri uri, MediaRangeList accept, Encoding encoding)
        {
            if (ioService == null)
                throw new ArgumentNullException(nameof(ioService));
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var request = new IORequest(uri, IOMethod.Get)
            {
                Accept = accept,
            };

            var response = ioService.Send(request);
            if (response.Status != IOStatus.Success)
                throw new IOException();

            if (!response.ContentType.Matches(accept))
                throw new IOException();

            if (encoding != null)
                return new IOTextReader(uri, response.Content, encoding);
            else
                return new IOTextReader(uri, response.Content);

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="ioService"></param>
        /// <param name="uri"></param>
        /// <param name="accept"></param>
        /// <returns></returns>
        public static TextReader Create(IIOService ioService, Uri uri, MediaRangeList accept)
        {
            if (ioService == null)
                throw new ArgumentNullException(nameof(ioService));
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return Create(ioService, uri, accept, null);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="ioService"></param>
        /// <param name="uri"></param>
        /// <param name="accept"></param>
        /// <returns></returns>
        public static TextReader Create(IIOService ioService, Uri uri, Encoding encoding)
        {
            if (ioService == null)
                throw new ArgumentNullException(nameof(ioService));
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return Create(ioService, uri, TXT_MEDIA_RANGES, encoding);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="ioService"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static TextReader Create(IIOService ioService, Uri uri)
        {
            if (ioService == null)
                throw new ArgumentNullException(nameof(ioService));
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return Create(ioService, uri, TXT_MEDIA_RANGES, null);
        }


        readonly Stream stream;
        readonly Uri uri;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="stream"></param>
        /// <param name="encoding"></param>
        IOTextReader(Uri uri, Stream stream, Encoding encoding)
            : base(stream, encoding)
        {
            this.uri = uri ?? throw new ArgumentNullException(nameof(uri));
            this.stream = stream ?? throw new ArgumentNullException(nameof(stream));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="stream"></param>
        IOTextReader(Uri uri, Stream stream)
            : base(stream)
        {
            this.uri = uri ?? throw new ArgumentNullException(nameof(uri));
            this.stream = stream ?? throw new ArgumentNullException(nameof(stream));
        }

    }

}
