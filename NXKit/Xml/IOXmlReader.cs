using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Xml;

using NXKit.IO;
using NXKit.Util;

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
            Contract.Requires<ArgumentNullException>(ioService != null);
            Contract.Requires<ArgumentNullException>(uri != null);

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
            Contract.Requires<ArgumentNullException>(ioService != null);
            Contract.Requires<ArgumentNullException>(uri != null);

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
            Contract.Requires<ArgumentNullException>(uri != null);
            Contract.Requires<ArgumentNullException>(reader != null);

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
            Contract.Requires<ArgumentNullException>(uri != null);
            Contract.Requires<ArgumentNullException>(stream != null);

            this.uri = uri;
        }

        public override string BaseURI
        {
            get { return uri.ToString(); }
        }

    }

}
