using System;
using System.Diagnostics.Contracts;
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
            Contract.Requires<ArgumentNullException>(ioService != null);
            Contract.Requires<ArgumentNullException>(uri != null);

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
            Contract.Requires<ArgumentNullException>(ioService != null);
            Contract.Requires<ArgumentNullException>(uri != null);

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
            Contract.Requires<ArgumentNullException>(ioService != null);
            Contract.Requires<ArgumentNullException>(uri != null);

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
            Contract.Requires<ArgumentNullException>(ioService != null);
            Contract.Requires<ArgumentNullException>(uri != null);

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
            Contract.Requires<ArgumentNullException>(uri != null);
            Contract.Requires<ArgumentNullException>(stream != null);
            Contract.Requires<ArgumentNullException>(encoding != null);

            this.uri = uri;
            this.stream = stream;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="stream"></param>
        IOTextReader(Uri uri, Stream stream)
            : base(stream)
        {
            Contract.Requires<ArgumentNullException>(uri != null);
            Contract.Requires<ArgumentNullException>(stream != null);

            this.uri = uri;
            this.stream = stream;
        }

    }

}
