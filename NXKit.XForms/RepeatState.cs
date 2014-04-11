using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using NXKit.Serialization;

namespace NXKit.XForms
{

    [XmlRoot("repeat")]
    public class RepeatState :
        IXmlSerializable
    {

        int index;
        XElement template;

        public int Index
        {
            get { return index; }
            internal set { index = value; }
        }

        public XElement Template
        {
            get { return template; }
            set { template = value; }
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            if (reader.MoveToContent() == XmlNodeType.Element &&
                reader.LocalName == "repeat")
            {
                index = int.Parse(reader["index"]);

                if (reader.ReadToDescendant("xml"))
                    template = XNodeAnnotationSerializer.Deserialize(new XDocument(
                        XElement.Load(
                            reader.ReadSubtree(),
                            LoadOptions.PreserveWhitespace | LoadOptions.SetBaseUri))).Root;
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("index", index.ToString());

            if (template != null)
            {
                writer.WriteStartElement("template");
                writer.WriteNode(XNodeAnnotationSerializer.Serialize(new XDocument(template)).CreateReader(), true);
                writer.WriteEndElement();
            }
        }

    }

}
