using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace NXKit.XForms
{

    [XmlRoot("repeat-item")]
    public class RepeatItemState :
        IXmlSerializable
    {

        int index;
        int modelItemId;

        internal int Index
        {
            get { return index; }
            set { index = value; }
        }

        internal int ModelItemId
        {
            get { return modelItemId; }
            set { modelItemId = value; }
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
                modelItemId = int.Parse(reader["model-item-id"]);
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("index", index.ToString());
            writer.WriteAttributeString("model-item-id", modelItemId.ToString());
        }

    }

}
