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

        int nextObjectId = 1;

        /// <summary>
        /// Gets the next available object ID.
        /// </summary>
        public int GetNextObjectId()
        {
            return nextObjectId++;
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
                nextObjectId = int.Parse(reader["next-object-id"]);
            }
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("next-object-id", nextObjectId.ToString());
        }

    }

}
