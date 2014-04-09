using System;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace NXKit.XForms
{

    /// <summary>
    /// Records additional information associated with a model item.
    /// </summary>
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

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
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
