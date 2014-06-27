using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

using NXKit.Serialization;

namespace NXKit.XForms
{

    [SerializableAnnotation]
    [XmlRoot("repeat")]
    public class RepeatState :
        IXmlSerializable
    {

        int index;
        XElement template;

        internal int Index
        {
            get { return index; }
            set { index = value; }
        }

        internal XElement Template
        {
            get { return template; }
            set { template = value; }
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            if (reader.MoveToContent() == XmlNodeType.Element &&
                reader.LocalName == "repeat")
            {
                index = int.Parse(reader["index"]);

                if (reader.ReadToDescendant("template"))
                    template = XElement.Load(reader.ReadSubtree())
                        .Elements()
                        .First();
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("index", index.ToString());

            if (template != null)
            {
                writer.WriteStartElement("template");
                writer.WriteNode(template.CreateReader(), true);
                writer.WriteEndElement();
            }
        }

    }

}
