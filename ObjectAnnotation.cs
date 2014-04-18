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
        bool init;

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
            this.init = false;
        }

        /// <summary>
        /// Gets the next available node ID.
        /// </summary>
        internal int Id
        {
            get { return id; }
        }

        /// <summary>
        /// Gets if the init phase has been run.
        /// </summary>
        internal bool Init
        {
            get { return init; }
            set { init = value; }
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
                init = bool.Parse(reader["init"]);
            }
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("id", id.ToString());
            writer.WriteAttributeString("init", init ? "true" : "false");
        }

    }

}
