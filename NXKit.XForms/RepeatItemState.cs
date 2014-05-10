using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

using NXKit.Serialization;

namespace NXKit.XForms
{

    [SerializableAnnotation]
    [XmlRoot("repeat-item")]
    public class RepeatItemState :
        IXmlSerializable
    {

        int modelItemId;
        int index;
        int size;

        internal int ModelItemId
        {
            get { return modelItemId; }
            set { modelItemId = value; }
        }

        internal int Index
        {
            get { return index; }
            set { index = value; }
        }

        internal int Size
        {
            get { return size; }
            set { size = value; }
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            if (reader.MoveToContent() == XmlNodeType.Element &&
                reader.LocalName == "repeat-item")
            {
                modelItemId = int.Parse(reader["model-item-id"]);
                index = int.Parse(reader["index"]);
                size = int.Parse(reader["size"]);
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("model-item-id", modelItemId.ToString());
            writer.WriteAttributeString("index", index.ToString());
            writer.WriteAttributeString("size", size.ToString());
        }

    }

}
