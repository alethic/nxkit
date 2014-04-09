using System;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace NXKit.XForms
{

    /// <summary>
    /// Serializable storage for a <see cref="UIBinding"/>'s state.
    /// </summary>
    [XmlRoot("ui-binding")]
    public class UIBindingState :
        IXmlSerializable
    {

        XName dataType;
        bool relevant;
        bool readOnly;
        bool required;
        bool valid;
        string value;

        bool dispatchValueChanged;
        bool dispatchReadOnly;
        bool dispatchReadWrite;
        bool dispatchRequired;
        bool dispatchOptional;
        bool dispatchEnabled;
        bool dispatchDisabled;
        bool dispatchValid;
        bool dispatchInvalid;

        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public bool DispatchValueChanged
        {
            get { return dispatchValueChanged; }
            set { dispatchValueChanged = value; }
        }

        public XName DataType
        {
            get { return dataType; }
            set { dataType = value; }
        }

        public bool Relevant
        {
            get { return relevant; }
            set { relevant = value; }
        }

        public bool DispatchEnabled
        {
            get { return dispatchEnabled; }
            set { dispatchEnabled = value; if (dispatchEnabled) dispatchDisabled = false; }
        }

        public bool DispatchDisabled
        {
            get { return dispatchDisabled; }
            set { dispatchDisabled = value; if (dispatchDisabled) dispatchEnabled = false; }
        }

        public bool ReadOnly
        {
            get { return readOnly; }
            set { readOnly = value; }
        }

        public bool DispatchReadOnly
        {
            get { return dispatchReadOnly; }
            set { dispatchReadOnly = value; if (dispatchReadOnly) dispatchReadWrite = false; }
        }

        public bool DispatchReadWrite
        {
            get { return dispatchReadWrite; }
            set { dispatchReadWrite = value; if (dispatchReadWrite) dispatchReadOnly = false; }
        }

        public bool Required
        {
            get { return required; }
            set { required = value; }
        }

        public bool DispatchRequired
        {
            get { return dispatchRequired; }
            set { dispatchRequired = value; if (dispatchRequired) dispatchOptional = false; }
        }

        public bool DispatchOptional
        {
            get { return dispatchOptional; }
            set { dispatchOptional = value; if (dispatchOptional) dispatchRequired = false; }
        }

        public bool Valid
        {
            get { return valid; }
            set { valid = value; }
        }

        public bool DispatchValid
        {
            get { return dispatchValid; }
            set { dispatchValid = value; if (dispatchValid) dispatchInvalid = false; }
        }

        public bool DispatchInvalid
        {
            get { return dispatchInvalid; }
            set { dispatchInvalid = value; if (dispatchInvalid) dispatchValid = false; }
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
            if (dataType != null)
                writer.WriteAttributeString("type", dataType.ToString());
            writer.WriteAttributeString("relevant", relevant ? "true" : "false");
            writer.WriteAttributeString("readonly", readOnly ? "true" : "false");
            writer.WriteAttributeString("required", required ? "true" : "false");
            writer.WriteAttributeString("valid", valid ? "true" : "false");
            if (value != null)
                writer.WriteAttributeString("value", value);
        }

    }

}
