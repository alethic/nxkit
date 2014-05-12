using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

using NXKit.Serialization;

namespace NXKit
{

    /// <summary>
    /// Stores various NXKit information on the <see cref="XObject"/>.
    /// </summary>
    [SerializableAnnotation]
    [XmlRoot("object")]
    public class ObjectAnnotation :
        IXmlSerializable
    {

        int id;
        bool init;
        bool load;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ObjectAnnotation()
            : this(0)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="id"></param>
        internal ObjectAnnotation(int id)
        {
            this.id = id;
            this.init = true;
            this.load = true;
        }

        /// <summary>
        /// Gets the next available node ID.
        /// </summary>
        internal int Id
        {
            get { return id; }
        }

        /// <summary>
        /// Gets if the init phase should be run.
        /// </summary>
        internal bool Init
        {
            get { return init; }
            set { init = value; }
        }

        /// <summary>
        /// Gets if the load phase should be run.
        /// </summary>
        internal bool Load
        {
            get { return load; }
            set { load = value; }
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            if (reader.MoveToContent() == XmlNodeType.Element &&
                reader.LocalName == "object")
            {
                id = int.Parse(reader["id"]);
                init = bool.Parse(reader["init"] ?? "false");
            }
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("id", id.ToString());
            if (init)
                writer.WriteAttributeString("init", "true");
        }

    }

}
