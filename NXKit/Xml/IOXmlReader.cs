using System;
using System.IO;
using System.Xml;

using NXKit.IO;
using NXKit.IO.Media;

namespace NXKit.Xml
{

    /// <summary>
    /// <see cref="IOXmlReader"/> instance that dispatches requests through the NXKit IO layer.
    /// </summary>
    public class IOXmlReader :
        XmlTextReader
    {

        static readonly MediaRangeList XML_MEDIA_RANGES = new[] {
            "text/xml",
            "application/xml",
            "application/octet-stream",
        };

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="ioService"></param>
        /// <param name="uri"></param>
        /// <param name="accept"></param>
        public static XmlReader Create(IIOService ioService, Uri uri, MediaRangeList accept)
        {
            if (ioService == null)
                throw new ArgumentNullException(nameof(ioService));
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            // default acceptable media types
            accept = !accept.IsEmpty ? accept : XML_MEDIA_RANGES;

            // format request
            var request = new IORequest(uri, IOMethod.Get)
            {
                Accept = accept,
            };

            // get response
            var response = ioService.Send(request);
            if (response.Status != IOStatus.Success)
                throw new XmlException();

            // acceptable response?
            if (!response.ContentType.Matches(accept))
                throw new XmlException();

            return new IOXmlReader(uri, response.Content);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="ioService"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static XmlReader Create(IIOService ioService, Uri uri)
        {
            if (ioService == null)
                throw new ArgumentNullException(nameof(ioService));
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return Create(ioService, uri, XML_MEDIA_RANGES);
        }


        readonly Uri uri;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="reader"></param>
        IOXmlReader(Uri uri, TextReader reader)
            : base(reader)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            this.uri = uri;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="stream"></param>
        IOXmlReader(Uri uri, Stream stream)
            : base(stream)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            this.uri = uri;
        }

        public override string BaseURI
        {
            get { return uri.ToString(); }
        }

    }

}
