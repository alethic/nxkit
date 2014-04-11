using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace NXKit
{

    /// <summary>
    /// Stores various NXKit information on the <see cref="XDocument"/>.
    /// </summary>
    [XmlRoot("document")]
    public class DocumentAnnotation :
        IXmlSerializable
    {

        bool initialized;
        int nextNodeId;

        /// <summary>
        /// Gets the next available node ID.
        /// </summary>
        public int GetNextNodeId()
        {
            return nextNodeId++;
        }

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
                nextNodeId = int.Parse(reader["next-node-id"]);
            }
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("initialized", initialized ? "true" : "false");
            writer.WriteAttributeString("next-node-id", nextNodeId.ToString());
        }

    }

}
