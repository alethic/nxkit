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

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ModelItemState()
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
