using System;
using System.Diagnostics.Contracts;
using System.Xml;

using NXKit.IO;
using NXKit.Util;

namespace NXKit.Xml
{

    /// <summary>
    /// <see cref="IOXmlReader"/> instance that dispatches requests through the NXKit IO layer.
    /// </summary>
    public class IOXmlReader :
        XmlReader
    {

        static readonly MediaRangeList XML_MEDIA_RANGES = new[] {
            "text/xml",
            "application/xml",
        };

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="ioService"></param>
        /// <param name="uri"></param>
        public static XmlReader Create(IIOService ioService, Uri uri)
        {
            Contract.Requires<ArgumentNullException>(ioService != null);
            Contract.Requires<ArgumentNullException>(uri != null);

            var request = new IORequest(uri, IOMethod.Get)
            {
                Accept = XML_MEDIA_RANGES,
            };

            var response = ioService.Send(request);
            if (response.Status != IOStatus.Success)
                throw new XmlException();

            if (!response.ContentType.Matches(XML_MEDIA_RANGES))
                throw new XmlException();

            return new IOXmlReader(uri, XmlReader.Create(response.Content));
        }


        readonly XmlReader reader;
        readonly Uri uri;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="reader"></param>
        IOXmlReader(Uri uri, System.Xml.XmlReader reader)
        {
            Contract.Requires<ArgumentNullException>(uri != null);
            Contract.Requires<ArgumentNullException>(reader != null);

            this.uri = uri;
            this.reader = reader;
        }

        public override int AttributeCount
        {
            get { return reader.AttributeCount; }
        }

        public override string BaseURI
        {
            get { return uri.ToString(); }
        }

        public override int Depth
        {
            get { return reader.Depth; }
        }

        public override bool EOF
        {
            get { return reader.EOF; }
        }

        public override string GetAttribute(int i)
        {
            return reader.GetAttribute(i);
        }

        public override string GetAttribute(string name, string namespaceURI)
        {
            return reader.GetAttribute(name, namespaceURI);
        }

        public override string GetAttribute(string name)
        {
            return reader.GetAttribute(name);
        }

        public override bool IsEmptyElement
        {
            get { return reader.IsEmptyElement; }
        }

        public override string LocalName
        {
            get { return reader.LocalName; }
        }

        public override string LookupNamespace(string prefix)
        {
            return reader.LookupNamespace(prefix);
        }

        public override bool MoveToAttribute(string name, string ns)
        {
            return reader.MoveToAttribute(name, ns);
        }

        public override bool MoveToAttribute(string name)
        {
            return reader.MoveToAttribute(name);
        }

        public override bool MoveToElement()
        {
            return reader.MoveToElement();
        }

        public override bool MoveToFirstAttribute()
        {
            return reader.MoveToFirstAttribute();
        }

        public override bool MoveToNextAttribute()
        {
            return reader.MoveToNextAttribute();
        }

        public override XmlNameTable NameTable
        {
            get { return reader.NameTable; }
        }

        public override string NamespaceURI
        {
            get { return reader.NamespaceURI; }
        }

        public override XmlNodeType NodeType
        {
            get { return reader.NodeType; }
        }

        public override string Prefix
        {
            get { return reader.Prefix; }
        }

        public override bool Read()
        {
            return reader.Read();
        }

        public override bool ReadAttributeValue()
        {
            return reader.ReadAttributeValue();
        }

        public override ReadState ReadState
        {
            get { return reader.ReadState; }
        }

        public override void ResolveEntity()
        {
            reader.ResolveEntity();
        }

        public override string Value
        {
            get { return reader.Value; }
        }

    }

}
