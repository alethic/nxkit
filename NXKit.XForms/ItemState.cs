using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace NXKit.XForms
{

    [XmlRoot("item")]
    public class ItemState :
        IXmlSerializable
    {

        Guid id;

        internal Guid Id
        {
            get { return id; }
            set { id = value; }
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            if (reader.MoveToContent() == XmlNodeType.Element &&
                reader.LocalName == "item")
                id = Guid.Parse((string)reader["id"]);
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("id", id.ToString());
        }

    }

}
