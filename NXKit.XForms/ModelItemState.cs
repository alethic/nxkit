using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

using NXKit.Serialization;

namespace NXKit.XForms
{

    /// <summary>
    /// Records additional information associated with a model item.
    /// </summary>
    [SerializableAnnotation]
    [XmlRoot("model-item")]
    public class ModelItemState :
        IXmlSerializable
    {

        XName type;
        bool? readOnly;
        bool? required;
        bool? relevant;
        bool? constraint;
        bool? valid;

        public XName Type
        {
            get { return type; }
            set { type = value; }
        }

        public bool? ReadOnly
        {
            get { return readOnly; }
            set { readOnly = value; }
        }

        public bool? Required
        {
            get { return required; }
            set { required = value; }
        }

        public bool? Relevant
        {
            get { return relevant; }
            set { relevant = value; }
        }

        public bool? Constraint
        {
            get { return constraint; }
            set { constraint = value; }
        }

        public bool? Valid
        {
            get { return valid; }
            set { valid = value; }
        }

        System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            if (reader.MoveToContent() == XmlNodeType.Element &&
                reader.LocalName == "model-item")
            {
                var type_ = reader["type"];
                if (type_ != null)
                    type = (XName)type_;

                var readOnly_ = reader["readonly"];
                if (readOnly_ != null)
                    readOnly = bool.Parse(readOnly_);

                var required_ = reader["required"];
                if (required_ != null)
                    required = bool.Parse(required_);

                var relevant_ = reader["relevant"];
                if (relevant_ != null)
                    relevant = bool.Parse(relevant_);

                var constraint_ = reader["constraint"];
                if (constraint_ != null)
                    constraint = bool.Parse(constraint_);

                var valid_ = reader["valid"];
                if (valid_ != null)
                    valid = bool.Parse(valid_);
            }
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            if (type != null)
                writer.WriteAttributeString("type", type.ToString());
            if (readOnly != null)
                writer.WriteAttributeString("readonly", (bool)readOnly ? "true" : "false");
            if (required != null)
                writer.WriteAttributeString("required", (bool)required ? "true" : "false");
            if (relevant != null)
                writer.WriteAttributeString("relevant", (bool)relevant ? "true" : "false");
            if (constraint != null)
                writer.WriteAttributeString("constraint", (bool)constraint ? "true" : "false");
            if (valid != null)
                writer.WriteAttributeString("valid", (bool)valid ? "true" : "false");
        }

    }

}
