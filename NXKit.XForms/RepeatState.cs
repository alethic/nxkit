using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace NXKit.XForms
{

    [XmlRoot("repeat")]
    public class RepeatState :
        IXmlSerializable
    {

        int index;

        public int Index
        {
            get { return index; }
            internal set { index = value; }
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
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("index", index.ToString());
        }

    }

}
