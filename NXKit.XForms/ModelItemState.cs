using System;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace NXKit.XForms
{

    /// <summary>
    /// Records additional information associated with a model item.
    /// </summary>
    public class ModelItemState :
        IXmlSerializable
    {

        int? id;
        XName type;
        bool? readOnly;
        bool? required;
        bool? relevant;
        bool? constraint;
        bool? valid;

        public int? Id
        {
            get { return id; }
            set { id = value; }
        }

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
                writer.WriteAttributeString("Type", type.ToString());
            if (readOnly != null)
                writer.WriteAttributeString("ReadOnly", readOnly.ToString());
            if (required != null)
                writer.WriteAttributeString("Required", required.ToString());
            if (relevant != null)
                writer.WriteAttributeString("Relevant", relevant.ToString());
            if (constraint != null)
                writer.WriteAttributeString("Constraint", constraint.ToString());
            if (valid != null)
                writer.WriteAttributeString("Valid", valid.ToString());
        }

    }

}
