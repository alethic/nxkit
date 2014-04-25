using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

using NXKit.Serialization;

namespace NXKit
{

    /// <summary>
    /// Stores various information on a <see cref="XDocument"/>.
    /// </summary>
    [SerializableAnnotation]
    [XmlRoot("document")]
    public class DocumentAnnotation :
        IXmlSerializable
    {

        int nextNodeId = 1;

        /// <summary>
        /// Gets the next available node ID.
        /// </summary>
        public int GetNextNodeId()
        {
            return nextNodeId++;
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
                nextNodeId = int.Parse(reader["next-node-id"]);
            }
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("next-node-id", nextNodeId.ToString());
        }

    }

}
