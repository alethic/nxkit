using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Records additional information associated with a model item.
    /// </summary>
    [Serializable]
    class ModelItemState :
        ISerializable
    {

        int? id;
        XName type;
        bool? readOnly;
        bool? required;
        bool? relevant;
        bool? constraint;
        bool? valid;

        bool clear;
        XElement newElement;
        string newValue;

        bool dispatchValueChanged;
        bool dispatchReadOnly;
        bool dispatchReadWrite;
        bool dispatchRequired;
        bool dispatchOptional;
        bool dispatchEnabled;
        bool dispatchDisabled;
        bool dispatchValid;
        bool dispatchInvalid;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        internal ModelItemState()
        {

        }

        /// <summary>
        /// Deserializes an instance.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        internal ModelItemState(SerializationInfo info, StreamingContext context)
        {
            Contract.Requires<ArgumentNullException>(info != null);

            id = (int?)info.GetValue("Id", typeof(int?));
            type = (XName)info.GetValue("Type", typeof(XName));
            readOnly = (bool?)info.GetValue("ReadOnly", typeof(bool?));
            required = (bool?)info.GetValue("Required", typeof(bool?));
            relevant = (bool?)info.GetValue("Relevant", typeof(bool?));
            constraint = (bool?)info.GetValue("Constraint", typeof(bool?));
            valid = (bool?)info.GetValue("Valid", typeof(bool?));
        }

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

        public bool DispatchValueChanged
        {
            get { return dispatchValueChanged; }
            set { dispatchValueChanged = value; }
        }

        public bool? ReadOnly
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

        public bool? Required
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

        public bool? Relevant
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

        /// <summary>
        /// Indicates that the model item is to be removed.
        /// </summary>
        public bool Clear
        {
            get { return clear; }
            set { clear = value; }
        }

        /// <summary>
        /// New <see cref="XElement"/> scheduled to be set as the value of the model item.
        /// </summary>
        public XElement NewContents
        {
            get { return newElement; }
            set { newElement = value; newValue = null; }
        }

        /// <summary>
        /// New <see cref="string"/> scheduled to be set as the value of the model item.
        /// </summary>
        public string NewValue
        {
            get { return newValue; }
            set { newValue = value; newElement = null; }
        }

        /// <summary>
        /// Clears any outstanding notifications.
        /// </summary>
        public void Reset()
        {
            dispatchValueChanged = false;
            dispatchReadOnly = false;
            dispatchReadWrite = false;
            dispatchRequired = false;
            dispatchOptional = false;
            dispatchEnabled = false;
            dispatchDisabled = false;
            dispatchValid = false;
            dispatchInvalid = false;

            newValue = null;
            newElement = null;
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Id", id);
            info.AddValue("Type", type);
            info.AddValue("ReadOnly", readOnly);
            info.AddValue("Required", required);
            info.AddValue("Relevant", relevant);
            info.AddValue("Constraint", constraint);
            info.AddValue("Valid", valid);
        }

    }

}
