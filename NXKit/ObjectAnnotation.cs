using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace NXKit
{

    /// <summary>
    /// Stores various NXKit information on the <see cref="XObject"/>.
    /// </summary>
    [XmlRoot("object")]
    public class ObjectAnnotation :
        IXmlSerializable
    {

        int id;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ObjectAnnotation()
        {
            this.id = 0;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="id"></param>
        internal ObjectAnnotation(int id)
        {
            this.id = id;
        }

        /// <summary>
        /// Gets the next available node ID.
        /// </summary>
        internal int Id
        {
            get { return id; }
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            if (reader.MoveToContent() == XmlNodeType.Element &&
                reader.LocalName == "object")
                id = int.Parse(reader["id"]);
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("id", id.ToString());
        }

    }

}
