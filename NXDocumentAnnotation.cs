using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace NXKit
{

    /// <summary>
    /// Stores various NXKit information on a <see cref="XDocument"/>.
    /// </summary>
    [XmlRoot("nx-document")]
    public class NXDocumentAnnotation :
        IXmlSerializable
    {

        bool initialized;

        /// <summary>
        /// Gets whether or not the document has been initialied.
        /// </summary>
        public bool Initialized
        {
            get { return initialized; }
            internal set { initialized = value; }
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            if (reader.MoveToContent() == XmlNodeType.Element &&
                reader.LocalName == "document")
            {
                initialized = bool.Parse(reader["initialized"]);
            }
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("initialized", initialized ? "true" : "false");
        }

    }

}
