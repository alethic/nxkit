using System;
using System.Diagnostics.Contracts;
using System.Xml;
using System.Xml.Linq;

namespace NXKit.IO
{

    /// <summary>
    /// Base wrapper around a <see cref="XmlReader"/> for <see cref="XNode"/> instances.
    /// </summary>
    public abstract class XNodeReaderBase :
        System.Xml.XmlReader
    {

        readonly XNode source;
        readonly XmlReader reader;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="source"></param>
        public XNodeReaderBase(XNode source)
        {
            Contract.Requires<ArgumentNullException>(source != null);

            this.source = source;
            this.reader = source.CreateReader(ReaderOptions.None);
        }

        /// <summary>
        /// Gets the source node being read from.
        /// </summary>
        public XNode Source
        {
            get { return source; }
        }

        public override int AttributeCount
        {
            get { return reader.AttributeCount; }
        }

        public override string BaseURI
        {
            get { return reader.BaseURI; }
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
